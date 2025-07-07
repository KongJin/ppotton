using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joypad : Selectable, IDragHandler
{
    [SerializeField] RectTransform Joystick;
    
    private Vector3 movement;//프로퍼티 외 접근금지
    public Vector3 Movement
    {
        get => movement;
        private set
        {
            Joystick.anchoredPosition = value;
            movement = value * 0.01f;
        }
    }

    
    private Vector3 cursorPoint;
    private float cursorDist;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {        
        cursorPoint = (Vector3)eventData.position - transform.position;
        cursorDist = Mathf.Clamp(cursorPoint.magnitude, 0, 100);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        Movement = Vector2.zero;
    }

    private void Update()
    {
        if (!IsPressed()) return; //안눌렷으면 패스
        Movement = cursorPoint.normalized * cursorDist;

    }

}
