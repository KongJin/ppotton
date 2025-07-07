using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PCInputSensor : IPlayerInputSensor
{
    //Queue<float> inputTimestamps = new();
    //private const float InputCheckSeconds = 5f;//n초안에 
    //private const int RequiredInputCount = 2;//m번 입력되면

    Transform repoter;
    public PCInputSensor()
    {
        repoter = Repoter.Instance.transform;
    }

    public void SetButtonPressing(ref NetworkButtons buttons)
    {
        buttons.Set(MyButtons.Jump, Input.GetKey(KeyCode.Space));
        buttons.Set(MyButtons.Attack, Input.GetKey(KeyCode.Mouse0));
        buttons.Set(MyButtons.Run, Input.GetKey(KeyCode.C));
        buttons.Set(MyButtons.ChangeMoveMode, Input.GetKey(KeyCode.V));        
    }
    Vector3 _movement;
    public void SetMoveDirection(ref Vector3 movement)
    {
        _movement = Vector3.zero;
        if (Input.GetKey(KeyCode.A)) _movement.x -= 1f;
        if (Input.GetKey(KeyCode.D)) _movement.x += 1f;
        if (Input.GetKey(KeyCode.W)) _movement.z += 1f;
        if (Input.GetKey(KeyCode.S)) _movement.z -= 1f;
        movement = repoter.TransformDirection(_movement.normalized);
    }
}
