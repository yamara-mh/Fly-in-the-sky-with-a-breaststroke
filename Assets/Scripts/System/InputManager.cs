using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Transform forwardTransformTemp;

    const int CTRL_PRE_POS_NUMBER = 5;

    public static Quaternion HMDrot { get; private set; }
    public static Transform forwardTransform { get; private set; }
    public static Transform HMDTransform { get; private set; }
    public static List<Transform> ctrlTrans { get; private set; }
    public static List<bool> ctrlIsRight { get; private set; }
    public static List<List<Vector3>> ctrlPrePos { get; private set; }
    public static List<bool> trigger { get; private set; }

    private void Awake()
    {
        ctrlTrans = new List<Transform>();
        ctrlIsRight = new List<bool>();
        ctrlPrePos = new List<List<Vector3>>();
        trigger = new List<bool>();

        forwardTransform = forwardTransformTemp;
    }

    private void FixedUpdate()
    {
        if (ctrlTrans.Count == 0) return;

        for (int i = 0; i < ctrlTrans.Count; i++)
        {
            //コントローラーの移動量
            ctrlPrePos[i].Insert(0, ctrlTrans[i].localPosition);
            if (ctrlPrePos[i].Count >= 5) ctrlPrePos[i].RemoveAt(4);
        }

        #region
        /*
        switch (Manager.gameType)
        {
            case Manager.GameType.PC:
                break;
            case Manager.GameType.RiftS:
                for (int i = 0; i < ctrlTrans.Count; i++)
                {
                    ctrlPrePos[i].Insert(0, ctrlTrans[i].position);
                    if (ctrlPrePos[i].Count > CTRL_PRE_POS_NUMBER) ctrlPrePos[i].RemoveAt(CTRL_PRE_POS_NUMBER);
                }
                break;
            case Manager.GameType.Go:
                break;
        }
        // */
        #endregion
    }

    private void Update()
    {
        if (ctrlTrans.Count == 0) return;

        //HMDrot = InputTracking.GetLocalRotation(XRNode.CenterEye);
        HMDrot = forwardTransformTemp.rotation;

        #region
        /*
        switch (Manager.gameType)
        {
            case Manager.GameType.PC:

                verRot.transform.Rotate(0, -Input.GetAxis("Mouse X"), 0);
                horRot.transform.Rotate(Input.GetAxis("Mouse Y"), 0, 0);
                HMDrot = horRot.rotation;

                //トリガー
                trigger[0] = Input.GetKey(KeyCode.Space);

                break;
            case Manager.GameType.RiftS:

                HMDrot = InputTracking.GetLocalRotation(XRNode.CenterEye);

                for (int i = 0; i < ctrlTrans.Count; i++)
                {
                    //トリガー
                    trigger[i] = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, i == 0 ? OVRInput.Controller.RTouch : OVRInput.Controller.LTouch);
                }
                break;
            case Manager.GameType.Go:

                HMDrot = InputTracking.GetLocalRotation(XRNode.CenterEye);

                for (int i = 0; i < ctrlTrans.Count; i++)
                {
                    //トリガー
                    trigger[i] = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);
                }
                break;
            case Manager.GameType.Vive:

                break;
        }
        // */
        #endregion
    }

    public static void SetHMDTransform(Transform _transform) { HMDTransform = _transform; }
    public static int GetCtrlCount() { return ctrlTrans.Count; }
    public static int SetCtrlTrans(Transform _transform, bool isRight)
    {
        ctrlTrans.Add(_transform);
        ctrlIsRight.Add(isRight);
        ctrlPrePos.Add(new List<Vector3>());
        trigger.Add(false);

        return ctrlTrans.Count - 1;
    }

    public static Quaternion GetHMDrot() { return HMDrot; }

    public static float GetCtrlDis(int number) { return Vector3.Distance(ctrlTrans[number].localPosition, ctrlPrePos[number][ctrlPrePos.Count]); }
    public static bool GetTrigger(int number) { return trigger[number]; }
    public static void SetTrigger(int number, bool _trigger) { trigger[number] = _trigger; }
    public static bool GetRight(int number) { return ctrlIsRight[number]; }
}