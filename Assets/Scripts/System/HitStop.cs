using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;

public class HitStop : MonoBehaviour
{
    public static bool stopping { get; private set; }

    public static void Stop(float second)
    {
        /*
        Time.timeScale = 0;
        stopping = true;

        DOVirtual.DelayedCall(second, () => {
            Time.timeScale = 1;
            stopping = false;
        });
        // */
    } 
}