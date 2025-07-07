using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repoter : MonoBehaviour
{
    [SerializeField] Camera mainCam;
    Transform target;
    private bool isInit;

    private static Repoter self;
    public static Repoter Instance => self;
    public void Initialize( Transform target)
    {
        this.target = target;
        isInit = true;         
    }

    private void Awake()
    {
        self = this;
        DontDestroyOnLoad(this);
        // 1) 커서를 창 안에 가둡니다.
        //Cursor.lockState = CursorLockMode.Confined;
    }

    private void LateUpdate()
    {
        if (!isInit || target == null) return;
        //target.position = Vector3.MoveTowards(target.position , transform.position, Time.deltaTime)  ;
        transform.position = target.position ;
        float mouseX = Input.GetAxis("Mouse X") ;
        transform.Rotate(0, mouseX, 0);

        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        // 드래그 중일 때만 deltaPosition.x를 사용
        if (touch.phase == TouchPhase.Moved)
        {
            // 화면 픽셀 이동량 → 회전량
            float deltaX = touch.deltaPosition.x;

            // Δ회전 = δx × 속도 × 화면 비율 보정
            float yaw = deltaX * Time.deltaTime;

            // Y축(위에서 볼 때 좌우) 회전 적용
            transform.Rotate(0f, yaw, 0f, Space.World);
        }
    }

}
