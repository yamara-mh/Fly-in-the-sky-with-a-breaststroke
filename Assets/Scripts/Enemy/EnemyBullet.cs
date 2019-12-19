using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField]
    private GameObject effect;
    [SerializeField]
    public Rigidbody _rigidbody;

    private const float aliveTime = 15;
    public float size, forcedTime, forcedSpeed;

    private void Start()
    {
        transform.localScale = Vector3.one * size;
        DOVirtual.DelayedCall(aliveTime, () => {
            Destroy(gameObject);
        });
    }

    void OnCollisionEnter(Collision collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            HitStop.Stop(0.1f);
            player.ForcedIvent(transform.forward, forcedTime, forcedSpeed * (1 / player.maxSpeed));
        }

        Destroy(gameObject);
        GameObject instant = Instantiate(effect, transform.position, Quaternion.Euler(transform.up));
        Destroy(instant, 1);
    }
}
