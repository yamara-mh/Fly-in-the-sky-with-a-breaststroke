using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ItemRing : MonoBehaviour
{
    [SerializeField]
    private float speedRate, time;
    [SerializeField]
    private Transform LookToObject;
    [SerializeField]
    private Collider _collider;
    [SerializeField]
    private Renderer _renderer;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip activateClip, shotClip;

    Quaternion quaternion;
    private bool isActivated, isShot;
    private float baseScale;

    void Start()
    {
        baseScale = transform.localScale.x;
        isActivated = false;
        isShot = false;
        transform.DOScale(baseScale, 0.75f).SetEase(Ease.OutSine);
        if (LookToObject) transform.LookAt(LookToObject.position);
        quaternion = transform.rotation;
        transform.DOLocalRotate(quaternion.eulerAngles + Vector3.forward * -360, 4, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);

        StartCoroutine("distanseCheck");
    }

    private IEnumerator distanseCheck()
    {
        yield return new WaitForSeconds(1);
        while (true)
        {
            float dis = Vector3.Distance(transform.position, Manager.playerPos);

            if (!isShot)
            {
                if (isActivated)
                {
                    if (dis >= 30)
                    {
                        isActivated = false;
                        transform.DOScale(baseScale, 0.75f).SetEase(Ease.OutSine);
                        transform.rotation = quaternion;
                        transform.DOLocalRotate(quaternion.eulerAngles + Vector3.forward * -360, 4, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
                    }
                }
                else
                {
                    if (dis <= 20)
                    {
                        isActivated = true;
                        transform.DOScale(baseScale * 2, 0.75f).SetEase(Ease.OutSine);
                        transform.rotation = quaternion;
                        transform.DOLocalRotate(quaternion.eulerAngles + Vector3.forward * -360, 1, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
                        audioSource.clip = activateClip;
                        audioSource.Play();
                    }
                }
            }

            //距離に応じてチェック頻度を変化
            if (!isShot)
            {
                yield return new WaitForSeconds(Mathf.Abs((dis - 25) * 0.01f));
            }
            else
            {
                isShot = false;
                yield return new WaitForSeconds(1.5f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (!player) return;

        isShot = true;
        if (Vector3.Dot((Manager.playerPos - transform.position).normalized, transform.forward) <= 0) player.ForcedIvent(transform.forward, time, speedRate);
        //if (transform.InverseTransformPoint(player.transform.position).z <= 0) player.RingAction(transform.forward, time, speedRate);
        else player.ForcedIvent(-transform.forward, time, speedRate);

        transform.DOScale(baseScale * 0.5f, 1).SetEase(Ease.InQuad);
        transform.rotation = quaternion;
        transform.DOLocalRotate(quaternion.eulerAngles + Vector3.forward * -1080, 1.5f, RotateMode.FastBeyond360).SetEase(Ease.OutQuad);
        audioSource.clip = shotClip;
        audioSource.Play();
    }
}
