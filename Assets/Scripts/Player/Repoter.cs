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
        // 1) Ŀ���� â �ȿ� ���Ӵϴ�.
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

        // �巡�� ���� ���� deltaPosition.x�� ���
        if (touch.phase == TouchPhase.Moved)
        {
            // ȭ�� �ȼ� �̵��� �� ȸ����
            float deltaX = touch.deltaPosition.x;

            // ��ȸ�� = ��x �� �ӵ� �� ȭ�� ���� ����
            float yaw = deltaX * Time.deltaTime;

            // Y��(������ �� �� �¿�) ȸ�� ����
            transform.Rotate(0f, yaw, 0f, Space.World);
        }
    }

}
