using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flypun : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private bool isRight;
    [SerializeField]
    private GameObject arm, jet, glove, gloveEffect;
    [SerializeField]
    private Renderer jetRenderer, gloveRenderer;
    [SerializeField]
    private Collider _collider;
    [SerializeField]
    private AudioSource jetActivateAudioSource, jetLoopAudioSource, gloveAoudioSource;
    [SerializeField]
    private AudioClip
        jetLoopClip;

    public int number {get; private set;}

    private void Awake()
    {
        number = -1;
        jetLoopAudioSource.Play();
    }

    void Update()
    {
        //コントローラーが見つかるまでUpdateを回さない
        if (number == -1)　{
            for (int i = 0; i < InputManager.GetCtrlCount(); i++) if (InputManager.GetRight(i) == isRight) number = i;
            if (number == -1)
            {
                _collider.enabled = false;
                return;
            }
        }

        transform.position = InputManager.ctrlTrans[number].position;
        transform.rotation = InputManager.ctrlTrans[number].rotation;

        if (InputManager.GetTrigger(number))
        {
            _collider.enabled = false;

            if (!jetRenderer.enabled)
            {
                jetRenderer.enabled = true;
                glove.SetActive(false);
            }
        }
        else
        {
            _collider.enabled = true;

            if (!glove.activeSelf)
            {
                glove.SetActive(true);
                jetRenderer.enabled = false;
            }
        }

        if (jetRenderer.enabled)
        {
            Vector3 angle = -player._rigidbody.velocity.normalized;
            jet.transform.localRotation = Quaternion.Euler(Mathf.Clamp(angle.x, 0, 0.5f), Mathf.Clamp(angle.y, 0, 0.5f), angle.z);

            jetLoopAudioSource.volume += InputManager.GetCtrlDis(number) * 0.05f;
            jetLoopAudioSource.pitch += InputManager.GetCtrlDis(number) * 0.25f;
        }

        jetLoopAudioSource.volume = Mathf.Max(0, Mathf.Min(0.05f, jetLoopAudioSource.volume - Time.deltaTime * 0.125f) );
        jetLoopAudioSource.pitch = Mathf.Max(0, Mathf.Min(1, jetLoopAudioSource.pitch - Time.deltaTime * 0.25f) );
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.transform.gameObject.GetComponent<Enemy>();
        if (enemy == null) return;

        HitStop.Stop(0.15f);
        float enemyMagnitude = player.rbMagnitude * Time.fixedDeltaTime + InputManager.GetCtrlDis(number) * (1 / Time.fixedDeltaTime) * (1 / InputManager.GetCtrlCount());
        enemy.TakeDamage(player.rbMagnitude + Mathf.Min(player.rbMagnitude, enemyMagnitude), 1);
        enemy.transform.position += (enemy.transform.position - (transform.position + Vector3.up * Manager.PLAYER_HEIGHT)).normalized;
        enemy._rigidbody.velocity = (enemy.transform.position - (transform.position + Vector3.up * Manager.PLAYER_HEIGHT)).normalized * player.rbMagnitude * enemyMagnitude;
        player._rigidbody.velocity = Vector3.zero;

        GameObject instant = Instantiate(gloveEffect, glove.transform.position + glove.transform.forward, glove.transform.rotation);
        Destroy(instant, 2);
    }
}