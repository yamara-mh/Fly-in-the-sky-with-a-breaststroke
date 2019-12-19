using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPrefabSetting : MonoBehaviour
{
    enum SettingType {
        EvenlyBox,
        EvenlySphere,
        RandomBox

    }

    [SerializeField]
    SettingType settingType;

    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private Vector3 center, range, rate;
    [SerializeField]
    private float rangeDistanse;
    [SerializeField]
    private int numberOfPut, limit;

    private int limitCount;

    void Start() {

        Vector3 trueCenter = new Vector3(center.x - range.x * 0.5f, center.y - range.y * 0.5f, center.z - range.z * 0.5f);

        switch (settingType)
        {
            case SettingType.EvenlyBox:

                for (float z = -range.z; z < range.z; z += rate.z)
                {
                    for (float y = -range.y; y < range.y; y += rate.y)
                    {
                        for (float x = -range.x; x < range.x; x += rate.x)
                        {
                            Instantiate(prefab, center +
                                new Vector3(
                                    x + Random.value * rate.x,
                                    y + Random.value * rate.y,
                                    z + Random.value * rate.z
                                    ),
                                Quaternion.identity, transform);

                            Debug.Log("X:" + x + ", Y:" + y +", Z:" + z);

                            limitCount++;
                            if (limitCount > limit) break;
                        }
                        if (limitCount > limit) break;
                    }
                    if (limitCount > limit) break;
                }
                break;
            case SettingType.RandomBox:

                while (numberOfPut > 0)
                {
                    numberOfPut--;
                    Instantiate(prefab,
                        new Vector3(
                            trueCenter.x + Random.value * range.x,
                            trueCenter.y + Random.value * range.y,
                            trueCenter.z + Random.value * range.z
                            ),
                        Quaternion.identity, transform);

                    limitCount++;
                    if (limitCount > limit) break;
                }
                break;
            case SettingType.EvenlySphere:

                Vector3 tempVector;

                while (numberOfPut > 0)
                {
                    numberOfPut--;
                    tempVector = new Vector3(Mathf.Sin(Random.value) * 2 - 1, Mathf.Sin(Random.value) * 2 - 1, Mathf.Sin(Random.value) * 2 - 1);
                    Instantiate(prefab, center + tempVector.normalized * rangeDistanse * Random.value,
                        Quaternion.identity, transform);

                    limitCount++;
                    if (limitCount > limit) break;
                }
                break;
        }
    }
}