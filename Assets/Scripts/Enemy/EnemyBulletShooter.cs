using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class EnemyBulletShooter : MonoBehaviour
{
    [SerializeField]
    private EnemyBullet bullet;

    [SerializeField]
    private float distance, shootingIntervalSecond, bulletSpeed, bulletSize, forcedTime, forcedSpeed;

    void Start()
    {
        Observable.Interval(TimeSpan.FromSeconds(shootingIntervalSecond)).Subscribe(_ =>
        {
            if (Vector3.Distance(Manager.playerPos, transform.position) <= distance)
            {
                EnemyBullet instant = Instantiate(bullet, transform.position, transform.rotation);
                instant.size = bulletSize;
                instant._rigidbody.velocity = transform.forward * bulletSpeed;
                instant.forcedTime = forcedTime;
                instant.forcedSpeed = forcedSpeed;
            }

        }).AddTo(this);
    }
}