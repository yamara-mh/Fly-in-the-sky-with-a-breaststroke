using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float hp, plusPlayerSpeed;
    private float invincibleTime;
    [SerializeField]
    public Rigidbody _rigidbody;
    [SerializeField]
    private GameObject destroyEffect;
 
    void FixedUpdate()
    {
        transform.LookAt(Manager.playerPos + Vector3.up * Manager.PLAYER_HEIGHT);

        if (invincibleTime > 0) invincibleTime -= Time.fixedDeltaTime;
    }

    public void TakeDamage(float damage, float _invincibleTime)
    {
        if (invincibleTime > 0) return;
        hp -= damage;
        invincibleTime = _invincibleTime;

        if (hp <= 0) Destruction();
    }

    private void Destruction()
    {
        Manager.player.maxSpeed += plusPlayerSpeed;
        HitStop.Stop(0.1f);
        Destroy(gameObject, 0.1f);
        GameObject instant = Instantiate(destroyEffect, transform.position, transform.rotation);
        Destroy(instant, 2);
    }
}