using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PointerActionMove : PointerAction
{
    public const float threshold = 0.1f;
    public const float pointerSpeed = 4f;
    public PointerController controller;
    public Vector3 movePos;


    public PointerActionMove(Action onFinish, Vector3 movePos) : base(onFinish)
    {
        if (onFinish != null) OnFinish += onFinish;
        this.movePos = movePos;
        Debug.Log(movePos);
    }

    public override void Execute(PointerController controller)
    {
        this.controller = controller;
    }

    public override void Update()
    {
        if (controller == null) return;
        if ((controller.transform.position - movePos).magnitude < threshold)
        {
            Finish();
        }
        controller.transform.position = Vector3.Lerp(controller.transform.position, movePos, Time.fixedDeltaTime * pointerSpeed);
    }
}
