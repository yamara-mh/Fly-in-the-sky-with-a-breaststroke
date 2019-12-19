using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWind : MonoBehaviour {

    AudioSource audioSource;
    ParticleSystem _particleSystem;
    [SerializeField] Player player;
    [SerializeField] GameObject particle;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        _particleSystem = particle.gameObject.GetComponent<ParticleSystem>();
    }

    void LateUpdate () {
        transform.localPosition = Vector3.up * 0.8f +
            player.GetRigidbody().velocity.normalized * 2;
        transform.LookAt(-Manager.playerPos);

        audioSource.volume = player.rbMagnitude * 0.005f;
        audioSource.pitch = 0.5f + player.rbMagnitude * 0.005f + (player.acceling * 4);

        if (player.rbMagnitude >= 10)
        {
            particle.SetActive(true);
            particle.transform.localPosition =
                Vector3.up * 0.8f + player._rigidbody.velocity * 0.2f;
            particle.transform.LookAt(Manager.playerPos);

            var em = _particleSystem.emission;
            em.rateOverTime = player.rbMagnitude * 0.2f;
        }
        else
        {
            particle.SetActive(false);
        }
    }
}
