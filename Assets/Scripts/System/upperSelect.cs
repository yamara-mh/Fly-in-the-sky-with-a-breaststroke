using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upperSelect : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> objects;
    [SerializeField]
    private float height;

    void Start()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if (objects[i].transform.position.y < height)
                Destroy(objects[i]);
        }
    }
}
