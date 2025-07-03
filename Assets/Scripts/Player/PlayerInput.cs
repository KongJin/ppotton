
using UnityEngine;
using Fusion;
public enum MyButtons//Buttons bool¹øÈ£
{
    
    Jump = 0,
    
    Attack = 1,
    
    ChangeMoveMode = 2,

    //
    GotoBattleButton = 3,
}
public struct PlayerInput :INetworkInput
{
    public NetworkButtons Buttons;
    public Vector3 Movement;
    public Vector3 AimDirection;
}
