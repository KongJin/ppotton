using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class MobileScreenSensor : IPlayerInputSensor
{
    public MobileScreenSensor(Transform camTransform)
    {

    }

    public void SetMoveDirection(ref Vector3 movement)
    {
        throw new NotImplementedException();
    }

    public void SetButtonPressing(ref NetworkButtons buttons)
    {
        throw new NotImplementedException();
    }
}
