
using UnityEngine;
using Fusion;
public enum MyButtons//Buttons bool¹øÈ£
{
    
    Jump ,
    
    Attack ,

    Run,
    
    ChangeMoveMode ,


    //
    GotoBattleButton ,
}
public struct PlayerInput :INetworkInput
{
    public NetworkButtons Buttons;
    public Vector3 Movement;
    public Vector3 AimDirection;
}
