using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    public enum playState//�÷��̾� ���� ����
    {
        Normal,
        Stun
    }
    float hp = 15f;
    public float playerSpeed;
    private float hAxis, vAxis;
    private bool wDown;
    private bool jDown;
    private bool iDown;
    public bool isAttacked = false;
    private bool isJump;
    private bool isDodge;
    private bool isStunned = false;
    public float speed;
    public float gravity;
    public float jumpSpeed = 15;
    private bool jumped = false;

    private Vector3 jumpVector;
    private Vector3 movingWay;

    private Transform bossWay;
    private Animator anim;
    private Rigidbody rigid;
    private CharacterController playerController;

    
    
    public bool isLoading;  // �ε����϶� �÷��̾� �Ͻ��������(������ �� ���� x).
    
    GameObject clickObject;  // �÷��̾ ��ȣ�ۿ� �� ������Ʈ�� �־��� ����.

    public GameManager gameManager; // ���ӸŴ���
    public playState p_State;
    public Transform enemypos;
    // Start is called before the first frame update
    private void Awake()
    {
        playerController = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        bossWay = GameObject.Find("Boss").transform;
        isJump = false;
        p_State = playState.Normal;//�÷��̾� ���¸� �⺻ ���·� 
    }

    private void Start()
    {
        speed = 6.0f;
        jumpSpeed = 9.8f;
        gravity = 9.8f;
    }

    // Update is called once per frame
    void Update()
    {
        Gravity();
        GetInput();
        Move();
        //Turn();
        Jump();
        //Dodge();
        Interaction();
    }

    void GetInput()
    {
        if (isStunned == true)
        {
            return;
        }
        else
        {
            hAxis = Input.GetAxis("Horizontal");
            vAxis = Input.GetAxis("Vertical");
            wDown = Input.GetButton("Walk");
            iDown = Input.GetKeyDown(KeyCode.E);
        }
    }

    private void Move()
    {
        if(isLoading)
            return;
        movingWay = new Vector3(hAxis, 0, vAxis).normalized;
        if (isAttacked)
        {
            movingWay = (bossWay.transform.position - gameObject.transform.position).normalized;
            playerController.Move(-movingWay * 100 * Time.deltaTime);
            Invoke("isAttackedFalse", 0.5f);
            return;
        }

        if (wDown)
        {
            playerController.Move(movingWay * playerSpeed * 0.3f * Time.deltaTime);
            anim.SetBool("IsWalk", wDown);
        }
        else
        {
            playerController.Move(movingWay * playerSpeed * Time.deltaTime);
            anim.SetBool("IsRun", movingWay != Vector3.zero);
        }
    }
    void JumpFalse()
    {
        jumped = false;
        isJump = false;
    }

    void Jump()
    {
        if (jumped)
        {
            playerController.Move(jumpVector * (float)3.5 * Time.deltaTime);
            Invoke("JumpFalse", 0.5f);
        }

        if (jDown && !isJump)
        {
            jumpVector.y = jumpSpeed;
            //cc.Move(Vector3.up * 35 * Time.deltaTime);
            //cc.Move(jumpVector);
            isJump = true;
            jumped = true;
        }
    }

    void Gravity()
    {
        if (jumped == true)
        {
            return;
        }

        jumpVector = new Vector3(0f, -20.0f, 0f);
        //jumpVector.y -= gravity;

        //movingWay = Vector3.zero;
        //movingWay.y -= gravity;
        playerController.Move(jumpVector * Time.deltaTime);
        //movingWay.y = 0;
    }

    /*void Dodge()
    {
        if (jDown && movingWay != Vector3.zero &&!isJump && !isDodge)
        {
            playerSpeed *= 2;
            isDodge = true;
            
            Invoke("DodgeOut", 3.0f);
        }
    }

    void DodgeOut()
    {
        playerSpeed *= (float)0.5;
        isDodge = false;
    } */

    void Interaction()
    {
        if (iDown)
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 2.5f)) // ������, ������ ���� ���, ��Ÿ�
            {
                if (hit.collider.CompareTag("Door"))
                {
                    clickObject = hit.collider.gameObject;
                    gameManager.Field_Change(clickObject);
                    isLoading = true;
                }
            }
            Debug.Log(clickObject.name);
        }
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
        if (other.gameObject.CompareTag("Zone"))
        {
            playerSpeed *= (float)0.5;
            Debug.Log("플레이어 속도 느려짐");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Zone"))
        {
            SlowOut();
        }
    }
    void SlowOut()
    {
        playerSpeed *= (float)2.0;
        //isStunned = false;
        Debug.Log("플레이어 속도 되돌아옴");
    }

    void isAttackedFalse()
    {
        isAttacked = false;
    }
    
    public void DamageAction(int damage)
    {
        hp -= damage;
        print("현재 남은 체력: " + hp);
    }
}