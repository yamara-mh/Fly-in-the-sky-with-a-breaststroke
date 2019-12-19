using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Basket : MonoBehaviour
{
    [SerializeField]
    private Enemy enemy;
    [SerializeField]
    private AudioSource audioSource;

    bool open;

    void Update()
    {
        if (!enemy && !open)
        {
            open = true;
            if (audioSource) audioSource.Play();
            transform.DOScaleY(1e-38f, 10).SetEase(Ease.OutSine).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        }
    }
}
