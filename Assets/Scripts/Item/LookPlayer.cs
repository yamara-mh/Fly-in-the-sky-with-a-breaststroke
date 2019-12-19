using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookPlayer : MonoBehaviour
{
    [SerializeField]
    private bool Yonly;

    Vector3 tempVector;

    void LateUpdate()
    {
        transform.LookAt(Manager.playerPos + Vector3.up * Manager.PLAYER_HEIGHT);

        if (Yonly) {
            tempVector = transform.rotation.eulerAngles;
            tempVector.y += 180;
            tempVector.x = 0;
            tempVector.z = 0;
            transform.rotation = Quaternion.Euler(tempVector);
        }
    }
}
