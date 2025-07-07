using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class MobileInputSensor : MonoBehaviour, IPlayerInputSensor
{
    [SerializeField] Joypad joypad;
    InputButton[] inputbuttons;
    
    private int inputButtonLen;
    private void Awake()
    {
        inputbuttons = GetComponentsInChildren<InputButton>();
        inputButtonLen = inputbuttons.Length;
    }
    Transform repoter;
    private void Start()
    {
        repoter = Repoter.Instance.transform;
    }

    //Ű �Է��� ȭ���� ��ư�� �˾ƸԾ����.
    //ȭ���� �־����.

    public void SetButtonPressing(ref NetworkButtons buttons)
    {
        for(int i=0; i<inputButtonLen; ++i)
        {
            InputButton button = inputbuttons[i];            
            var item = button.GetButtonState();            
            buttons.Set(item.number, item.isPress);
        }
    }

    Vector3 _movement;
    public void SetMoveDirection(ref Vector3 movement)
    {
        _movement.x = joypad.Movement.x;        
        _movement.z = joypad.Movement.y;

        movement = repoter.TransformDirection(_movement);
    }
}
