using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapView : MonoBehaviour
{
    [SerializeField]
    private Transform pointer;

    private const float rate = 1 / 898.245614035f;

    void FixedUpdate()
    {
        pointer.localPosition = new Vector3(Manager.playerPos.x, Manager.playerPos.z, 0);
        pointer.localPosition *= rate;
        pointer.localRotation = Quaternion.Euler(0, 0, -InputManager.HMDrot.eulerAngles.y);
        Debug.Log(InputManager.HMDrot);
    }
}