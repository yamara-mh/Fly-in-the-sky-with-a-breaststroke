using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject resultObject;
    [SerializeField] private TextMeshPro meter, time, rankText;
    [SerializeField] private Player player;
    [SerializeField] private Image panel, water;

    [SerializeField] private AudioSource audioSource;

    private float
        resultCount = 0;

    [SerializeField]
    private Transform pointer;
    private const float rate = 1 / 898.245614035f,
        RANK_VIEW_WAIT_TIME = 2,
        RANK_VIEW_TIME_PLUS = 0.1f;

    private int rankCount = 0;


    void FixedUpdate()
    {
        pointer.localPosition = new Vector3(Manager.playerPos.x, Manager.playerPos.z, 0);
        pointer.localPosition *= rate;
        pointer.localRotation = InputManager.HMDrot * Quaternion.Euler(Vector3.forward);
    }

    void Update()
    {
        if (Manager.gaming)
        {
            time.text = "Time : " + Mathf.Max(0, Manager.gameTime * Manager.GAME_TIME).ToString("0");
        }
        else if (Manager.result)
        {
            time.text = "";
        }
        meter.text = player.rbMagnitude.ToString("0") + "km/h";


        if (InputManager.HMDTransform.position.y < 150)
        {
            water.color = new Color(0, 0, 0.5f, 0.5f);
        }
        else
        {
            water.color = new Color(0, 0, 0.5f, 0);
        }

        if (Manager.gameTime * Manager.GAME_TIME < 1)
        {
            //フェードイン
            panel.color = new Color(1, 1, 1, 1 - Manager.gameTime * Manager.GAME_TIME);
        }

        if (Manager.result)
        {
            resultCount += Time.deltaTime;
            resultObject.SetActive(true);

            //フェードアウト
            panel.color = new Color(1, 1, 1, 1 - resultCount);

            //スコア
            #region
            if (rankCount == 0 && player.maxSpeed > 50 && resultCount >= 1 + RANK_VIEW_WAIT_TIME)
            {
                rankCount++;
                rankText.text = "E";
                audioSource.pitch = 0.8f;
                audioSource.Play();
            }
            if (rankCount == 1 && player.maxSpeed > 65 && resultCount >= 1.3f + RANK_VIEW_WAIT_TIME)
            {
                rankCount++;
                rankText.text = "D";
                audioSource.pitch = 0.85f;
                audioSource.Play();
            }
            if (rankCount == 2 && player.maxSpeed > 80 && resultCount >= 1.7f + RANK_VIEW_WAIT_TIME)
            {
                rankCount++;
                rankText.text = "C";
                audioSource.pitch = 0.9f;
                audioSource.Play();
            }
            if (rankCount == 3 && player.maxSpeed > 95 && resultCount >= 2.2f + RANK_VIEW_WAIT_TIME)
            {
                rankCount++;
                rankText.text = "B";
                audioSource.pitch = 0.95f;
                audioSource.Play();
            }
            if (rankCount == 4 && player.maxSpeed > 110 && resultCount >= 2.8f + RANK_VIEW_WAIT_TIME)
            {
                rankCount++;
                rankText.text = "A";
                audioSource.pitch = 1;
                audioSource.Play();
            }
            if (rankCount == 5 && player.maxSpeed > 125 && resultCount >= 3.5f + RANK_VIEW_WAIT_TIME)
            {
                rankCount++;
                rankText.text = "S";
                audioSource.pitch = 1.05f;
                audioSource.Play();
            }
            if (rankCount == 6 && player.maxSpeed > 140 && resultCount >= 4 + RANK_VIEW_WAIT_TIME)
            {
                rankCount++;
                rankText.text = "SS";
                audioSource.pitch = 1.1f;
                audioSource.Play();
            }
            if (rankCount == 7 && player.maxSpeed > 155 && resultCount >= 4.5f + RANK_VIEW_WAIT_TIME)
            {
                rankCount++;
                rankText.text = "SSS";
                audioSource.pitch = 1.15f;
                audioSource.Play();
            }
            if (rankCount == 8 && player.maxSpeed > 170 && resultCount >= 5 + RANK_VIEW_WAIT_TIME)
            {
                rankCount++;
                rankText.text = "SSSS";
                audioSource.pitch = 1.2f;
                audioSource.Play();
            }
            if (rankCount == 9 && player.maxSpeed > 185 && resultCount >= 5.5f + RANK_VIEW_WAIT_TIME)
            {
                rankCount++;
                rankText.text = "SSSSS";
                audioSource.pitch = 1.25f;
                audioSource.Play();
            }
            if (rankCount == 10 && player.maxSpeed > 200 && resultCount >= 6 + RANK_VIEW_WAIT_TIME)
            {
                rankCount++;
                rankText.text = "SSSSSS";
                audioSource.pitch = 1.35f;
                audioSource.Play();
            }
            #endregion
        }
    }
}