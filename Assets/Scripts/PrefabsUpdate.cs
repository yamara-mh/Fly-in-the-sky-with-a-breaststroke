using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabsUpdate : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> objects;
    [SerializeField]
    private GameObject prefab;

    void Start()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            Instantiate(prefab, objects[i].transform.position, objects[i].transform.rotation, transform);
            Destroy(objects[i]);
        }
    }
}