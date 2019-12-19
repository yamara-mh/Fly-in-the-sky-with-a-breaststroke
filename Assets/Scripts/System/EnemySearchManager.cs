using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySearchManager : MonoBehaviour
{
    [SerializeField]
    private List<Transform> enemyTransforms;

    [SerializeField]
    private GameObject Arrow;

    public Transform targetTransform;
    private float nearestDistance;

    void Start()
    {
        StartCoroutine("distanseCheck");
    }

    void Update()
    {
        if (targetTransform == null)
        {
            Arrow.SetActive(false);
            return;
        }

        Arrow.transform.LookAt(targetTransform.position);
    }

    private IEnumerator distanseCheck()
    {
        yield return new WaitForSeconds(0.25f);

        while (true)
        {
            nearestDistance = Mathf.Infinity;

            for (int i = 0; i < enemyTransforms.Count; i++)
            {
                if (enemyTransforms[i] == null)
                {
                    enemyTransforms.RemoveAt(i);
                    if (enemyTransforms.Count == 0) Arrow.SetActive(false);
                    continue;
                }

                if (Vector3.SqrMagnitude(Manager.playerPos - enemyTransforms[i].position) < nearestDistance)
                {
                    Arrow.SetActive(true);
                    nearestDistance = Vector3.SqrMagnitude(Manager.playerPos - enemyTransforms[i].position);
                    targetTransform = enemyTransforms[i];
                }
            }

            //BGM変化
            if (Arrow && nearestDistance < BGMPlayer.CHANGE_ACT_START_DISTANCE)
            {
                BGMPlayer.BattleStart();
            }
            else
            {
                BGMPlayer.BattleEnd();
            }

            yield return new WaitForSeconds(3);
        }
    }
}
