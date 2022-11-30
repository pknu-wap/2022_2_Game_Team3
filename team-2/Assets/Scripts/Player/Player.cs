using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int life = 2; // 생명력.
    public bool live;
    
    float hAxis, vAxis;     // 어느 방향으로 이동할 것인지 입력받아줄 변수.
    float playerSpeed = 10;  // 플레이어의 기본 이동속도.
    float jumpPower = 8.0f;    // 플레이어의 점프력

    bool isJump;            // 점프 중인지 확인해줄 bool변수.

    bool jDown;             // 점프 키
    bool iDown;             // 상호작용 키
    bool pauseDown;         // pause Button
        
    public bool isLoading;  // 로딩중일때 플레이어 일시정지기능(움직임 및 점프 x).

    Vector3 movingWay;      // 플레이어가 나아갈 방향

    Rigidbody rigid;        // 플레이어의 리지드바디.

    public GameObject clickObject;  // 플레이어가 상호작용 할 오브젝트를 넣어줄 변수.

    public GameManager gameManager; // 게임매니저
    public SystemManager systemManager; // 시스템 매니저
    public RSP rsp;

    //public GameObject feildPointObj; // 필드이동에 사용되는 포인트 지점 체크하는 변수 

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        isJump = false; 
        live = true;
    }

    void Update()
    {
        GetInput();
        Move();
        Jump();
        Interaction();
        if(pauseDown == true)
        {
            gameManager.PauseFunc();
        }
    }

    void GetInput()
    {
        hAxis = systemManager.isAction ? 0 : Input.GetAxis("Horizontal");
        vAxis = systemManager.isAction ? 0 : Input.GetAxis("Vertical");
        jDown = systemManager.isAction ? false : Input.GetButtonDown("Jump");
        iDown = systemManager.isSelectInformation ? false : Input.GetKeyDown(KeyCode.E);
        pauseDown = Input.GetKeyDown(KeyCode.Escape);
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
        if(iDown && clickObject != null && !isLoading)
        {
            if (clickObject.CompareTag("Door"))
            {
                Debug.Log("Interaction + " + clickObject.name);
                isLoading = true;
                gameManager.Field_Change(clickObject);
            }
            else if(clickObject.CompareTag("Artifact"))
            {
                gameManager.Get_Artifact(clickObject);
                clickObject = null;
            }
            else if(clickObject.CompareTag("Treasure"))
            {
                clickObject.GetComponent<Treasure>().OpenBox();
                clickObject = null;
            }
            else if(clickObject.CompareTag("NPC"))
            {
                systemManager.SetTextPanel(clickObject.gameObject);
            }
            else if(clickObject.CompareTag("Broken_Door"))
            {
                systemManager.PlayerText(clickObject);
            }
            else if(clickObject.name == "BossRoom In Point")
            {
                Debug.Log(clickObject.name);
                gameManager.BossInRoom();
            }
        }
        /*
        if (iDown && clickObject.CompareTag("Door") && !isLoading)
        {
            Debug.Log("Interaction + " + clickObject.name);
            isLoading = true;
            gameManager.Field_Change(clickObject);
        }
        else if(iDown && clickObject.CompareTag("Artifact"))
        {
            gameManager.Get_Artifact(clickObject);
        }
        else if(iDown && clickObject.CompareTag("Treasure"))
        {
            clickObject.GetComponent<Treasure>().OpenBox();
        }
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
        if(other.gameObject.CompareTag("Monster")&& live == true)
        {
            live = false;
            gameManager.GameOver();
        }
        else if(other.gameObject.CompareTag("Flame")&&live == true)
        {
            live = false;
            gameManager.GameOver();
        }
        else if(other.gameObject.name == "Paper" || other.gameObject.name == "Scissor" || other.gameObject.name == "Rock")
        {
            Debug.Log("내가 밟고 있는거 " + other.gameObject.name);
            rsp.SetPlayerRCP(other.gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        clickObject = null;
        if(other.name == "StartMessage")
        {
            systemManager.StartMessage();
            other.gameObject.SetActive(false);
            PlayerPrefs.SetInt("spawnPoint", 1);
        }
    }
}
