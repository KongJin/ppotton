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

    private NetworkButtons bottonsPrevious;
    private PlayerInput curInput;

    public bool GetPressed(MyButtons key)=> curInput.Buttons.WasPressed(bottonsPrevious, key);
    public bool GetPress(MyButtons key)=>curInput.Buttons.IsSet(key);
    
    public override void FixedUpdateNetwork()
    {
        DeltaTime = Runner.DeltaTime;
        bottonsPrevious = curInput.Buttons;
        if (GetInput(out curInput) == false) return;
        curInput.Movement *= curInput.Buttons.IsSet(MyButtons.Run) ? 1 : 0.5f;
        Movement = Vector3.Lerp(Movement, curInput.Movement, DeltaTime * 10);
    }

    public override void Spawned()
    {
        if(Object.HasStateAuthority) Instance = this;
    }

}
