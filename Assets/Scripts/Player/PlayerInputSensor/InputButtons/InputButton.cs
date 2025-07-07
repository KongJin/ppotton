using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputButton : Selectable
{
    [SerializeField] private MyButtons number;    

    public (MyButtons number, bool isPress) GetButtonState() => (number, IsPressed());

    
}
