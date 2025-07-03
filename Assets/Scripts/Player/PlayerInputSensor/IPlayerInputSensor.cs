using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public interface IPlayerInputSensor
{
    //카메라가 보고있는 방향과 키입력을 계산한 월드기준 방향 제공
    void SetMoveDirection(ref Vector3 movement);
    void SetButtonPressing(ref NetworkButtons buttons);

    static IPlayerInputSensor GetInputSensor()
    {

        return new PCKeyboardInput();

        //return new MobileScreenSensor(camTransform);

    }
}
