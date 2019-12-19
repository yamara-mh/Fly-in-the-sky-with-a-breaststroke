using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

    public const float
        GAME_TIME = 249,
        PLAYER_HEIGHT = 1.6f;
    public static float gameTime { get; private set; }
    public static Player player;
    public static Vector3 playerPos;
    public static bool gaming, result;
    [SerializeField]
    private List<GameObject> obstacles;

    public enum GameType
    {
        PC,
        RiftS,
        Go,
        Vive,
    }

    [SerializeField] public static GameType gameType = GameType.Vive;

    private void Start()
    {
        gameTime = 1;
    }

    private void Update()
    {
        //Esc終了
        if (Input.GetKey(KeyCode.Escape)) Application.Quit();

        if (gaming)
        {
            gameTime -= Time.deltaTime / GAME_TIME;

            if (gameTime <= 0.0041667f)
            {
                if (gameTime <= 0)
                {
                    gaming = false;
                    result = true;

                    player.transform.position = new Vector3(3000, 1025, 3000);   

                    for (int i = 0; i < obstacles.Count; i++)
                    {   //障害物除去
                        Destroy(obstacles[i]);
                    }
                }
            }
        }
        else if (result)
        {
        }
        else
        {
            //スペースキーでゲーム開始
            if (Input.GetKey(KeyCode.Space)) gaming = true;
        }

        //デバッグ
        if (!Input.GetKey(KeyCode.D)) return;

        //フリーフライトモード
        if (Input.GetKey(KeyCode.F))
        {
            for (int i = 0; i < obstacles.Count; i++)
            {
                Destroy(obstacles[i]);
            }
        }
        //時間増加
        if (Input.GetKey(KeyCode.T))
        {
            if (Input.GetKey(KeyCode.M))
            {   //時間減少
                gameTime -= Time.deltaTime / GAME_TIME * 50;
                return;
            }

            gameTime += Time.deltaTime / GAME_TIME * 25;
        }

        //最高速度上昇
        if (Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.M))
            {   //最高速度低下
                player.maxSpeed -= 20 * Time.deltaTime;
                return;
            }

            player.maxSpeed += 20 * Time.deltaTime;
        }

        //プレイヤーの動きを止める
        if (Input.GetKey(KeyCode.Z))
        {
            player._rigidbody.velocity = Vector3.zero;
        }
    }
}
