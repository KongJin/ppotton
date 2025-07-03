using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;

public class PlayerInputContainer : NetworkBehaviour
{
    public static PlayerInputContainer Instance { get; private set; }
    public Vector3 Movement { get; private set; }
    public float DeltaTime { get; private set; }

    NetworkButtons bottonsPrevious;
    PlayerInput curInput;

   
    public bool GetPressed(MyButtons key)
    {
        return curInput.Buttons.WasPressed(bottonsPrevious, key);
    }

    public bool GetPress(MyButtons key)=>curInput.Buttons.IsSet(key);
    
    public override void FixedUpdateNetwork()
    {
        DeltaTime = Runner.DeltaTime;
        bottonsPrevious = curInput.Buttons;
        if (GetInput(out curInput) == false) return;
        Movement = Vector3.Lerp(Movement, curInput.Movement, DeltaTime * 10);
    }

    private void Awake()
    {
        Instance = this;
    }


}
