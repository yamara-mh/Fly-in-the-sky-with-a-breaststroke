using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ItemSpeedUp : MonoBehaviour {

    private const float speedUpValue = 4;

    [SerializeField]
    private ParticleSystem particle;
    [SerializeField]
    private Collider _collider;
    [SerializeField]
    private Renderer _renderer;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private LineRenderer lineRenderer;

	void Start () {
        StartCoroutine("distanseCheck");

        //アニメーション
        transform.DORotate(new Vector3(0, 360, 0), 4, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        Sequence sequence = DOTween.Sequence()
            .Append(transform.DOMoveY(transform.position.y + 1, 2).SetEase(Ease.InOutSine))
            .Append(transform.DOMoveY(transform.position.y, 2).SetEase(Ease.InOutSine).SetDelay(2))
            .SetLoops(-1, LoopType.Restart);
        sequence.Play();

        lineRenderer.SetPosition(0, transform.position + Vector3.up * 100);
        lineRenderer.SetPosition(1, transform.position + Vector3.down * 25);
    }

    private IEnumerator distanseCheck()
    {
        while (true)
        {
            float dis = Vector3.Distance(transform.position, Manager.playerPos);

            //距離によってLineの表示非表示を切り替える
            lineRenderer.enabled = dis <= 1000;
            //距離に応じてチェック頻度を変化
            yield return new WaitForSeconds(Mathf.Abs((dis - 1000) * 0.01f));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            other.GetComponent<Player>().AddNormalSpeed(speedUpValue);
            particle.Play();
            _collider.enabled = false;
            _renderer.enabled = false;
            audioSource.Play();
            transform.DOScale(0, 0.3f);
            Destroy(gameObject, 1);
        }
    }
}