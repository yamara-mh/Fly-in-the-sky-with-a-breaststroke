using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Valve.VR;

public class SetController : MonoBehaviour
{
    [SerializeField] public bool isRight, isHMD;
    public int number;

    private SteamVR_Action_Boolean actionToGrip = SteamVR_Actions._default.GrabPinch;

    void Start()
    {
        if (isHMD)
        {
            InputManager.SetHMDTransform(transform);
        }
        else
        {
            number = InputManager.SetCtrlTrans(transform, isRight);
        }
    }

    void Update()
    {
        if (isHMD)
        {

        }
        else
        {
            switch (Manager.gameType)
            {
                case Manager.GameType.PC:
                    if (Input.GetKeyDown(KeyCode.W)) transform.DOLocalMoveZ(1, 0.5f);
                    if (Input.GetKeyDown(KeyCode.S)) transform.DOLocalMoveZ(-1, 0.5f);
                    break;
                case Manager.GameType.Vive:
                    if (actionToGrip.GetState(isRight ? SteamVR_Input_Sources.RightHand : SteamVR_Input_Sources.LeftHand))
                    {
                        InputManager.SetTrigger(number, true);
                    }
                    else
                    {
                        InputManager.SetTrigger(number, false);
                    }
                        /*
                        var trackedObject = GetComponent<SteamVR_TrackedObject>();
                        var device = SteamVR_Controller.Input((int)trackedObject.index);
                        Debug.Log("Vive");
                        if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
                        {
                            InputManager.SetTrigger(number, true);
                            Debug.Log("Trigger");
                        }
                        else
                        {
                            InputManager.SetTrigger(number, false);
                        }
                        // */
                        break;
            }
        }
    }
}
