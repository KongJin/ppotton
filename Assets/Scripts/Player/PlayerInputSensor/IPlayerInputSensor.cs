using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public interface IPlayerInputSensor
{
    //ī�޶� �����ִ� ����� Ű�Է��� ����� ������� ���� ����
    void SetMoveDirection(ref Vector3 movement);
    void SetButtonPressing(ref NetworkButtons buttons);

    static IPlayerInputSensor GetInputSensor()
    {

        return new PCKeyboardInput();

        //return new MobileScreenSensor(camTransform);

    }
}
