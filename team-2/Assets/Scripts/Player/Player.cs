using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int life = 2; // 생명력.

    float hAxis, vAxis;     // 어느 방향으로 이동할 것인지 입력받아줄 변수.
    float playerSpeed = 10;  // 플레이어의 기본 이동속도.
    float jumpPower = 10;    // 플레이어의 점프력

    bool isJump;            // 점프 중인지 확인해줄 bool변수.

    bool jDown;             // 점프 키
    bool iDown;             // 상호작용 키
        
    public bool isLoading;  // 로딩중일때 플레이어 일시정지기능(움직임 및 점프 x).

    Vector3 movingWay;      // 플레이어가 나아갈 방향

    Rigidbody rigid;        // 플레이어의 리지드바디.

    public GameObject clickObject;  // 플레이어가 상호작용 할 오브젝트를 넣어줄 변수.

    public GameManager gameManager; // 게임매니저

    //public GameObject feildPointObj; // 필드이동에 사용되는 포인트 지점 체크하는 변수 

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        isJump = false; 
    }

    void Update()
    {
        GetInput();
        Move();
        Jump();
        Interaction();
    }

    void GetInput()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        jDown = Input.GetButtonDown("Jump");
        iDown = Input.GetKeyDown(KeyCode.E);
    }

    private void Move()
    {
        if (!isLoading)
        {
            movingWay = new Vector3(hAxis, 0, vAxis).normalized;

            transform.Translate(movingWay * playerSpeed * Time.deltaTime);
            //transform.position += movingWay * playerSpeed * Time.deltaTime;
        }
    }

    void Jump()
    {
        if (jDown && !isJump && !isLoading)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isJump = true;
        }
    }

    void Interaction()
    {
        if (iDown && clickObject.CompareTag("Door"))
        {
            Debug.Log("Interaction + " + clickObject.name);
            gameManager.Field_Change(clickObject);
            isLoading = true;
        }
        else if(iDown && clickObject.CompareTag("Artifact"))
        {
            gameManager.Get_Artifact(clickObject);
        }
        /*
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2.5f)) // 레이저, 레이저 맞춘 대상, 사거리
        {
            if (hit.collider.CompareTag("Door"))
            {
                clickObject = hit.collider.gameObject;
                gameManager.Field_Change(clickObject);
                isLoading = true;
            }
        }
        Debug.Log(clickObject.name);
        */
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isJump = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        clickObject = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        clickObject = null;
    }
}
