using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class BossPlayer : MonoBehaviour
{
    public enum playState//�÷��̾� ���� ����
    {
        Normal,
        Stun
    }
    public float hp = 15f;
    public int attackPower = 3;//플레이어의 공격력
    private float hAxis, vAxis;
    private bool jDown;
    private bool iDown;
    
    public float playerSpeed;

    public bool isAttacked = false;
    private bool isDodge;

    private Vector3 jumpVector;
    private Vector3 movingWay;

    private Transform bossWay;
    private Animator anim;
    private CharacterController playerController;
    public Transform attackPoint; //플레이어의 공격이 나가는 위치

    
    
    public bool isLoading;  // �ε����϶� �÷��̾� �Ͻ��������(������ �� ���� x).
    
    GameObject clickObject;  // �÷��̾ ��ȣ�ۿ� �� ������Ʈ�� �־��� ����.

    public GameManager gameManager; // ���ӸŴ���
    public playState p_State;
    public Transform enemypos;
    float currentTime;// 공격 딜레이용 변수
    float attackDelay = 1;// 공격 간 딜레이값
    
    private BoxCollider boxTest;

    private GameObject boss;
    // Start is called before the first frame update
    private void Awake()
    {
        boss = GameObject.Find("Boss");
        boxTest = boss.GetComponent<BoxCollider>();
        playerController = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        bossWay = GameObject.Find("Boss").transform;
        p_State = playState.Normal;//�÷��̾� ���¸� �⺻ ���·� 
    }

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(p_State == playState.Stun)//스턴(적 위치로 플레이어 끌어당기기) 상태가 되면 실행
        {
            Vector3 way = (enemypos.position - transform.position).normalized;//플레이어를 당겨오기 위해 벡터로 방향을 계산하고
            transform.Translate(way * 2 * Time.deltaTime, Space.World);//스턴 동안 해당 값만큼 당겨온다.
            return;//플레이어가 스턴 상태면 위의 내용먼 실행시키고 리턴->사용자의 움직임 입력을 받지못함
        }
        Attack();//공격 처리 함수
        Gravity();
        GetInput();
        Move();
        Dodge();
        Interaction();
    }

    void GetInput()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        iDown = Input.GetKeyDown(KeyCode.E);
        jDown = Input.GetKeyDown(KeyCode.Space);
    }

    private void Attack()///공격 수행 함수
    {
        currentTime += Time.deltaTime;//공격 쿨타임 
        if (currentTime > attackDelay)//공격 쿨타임이 차면 공격 가능
        {
            if (Input.GetMouseButtonDown(0))//공격 쿨타임이 찬 상태에서 마우스 좌클릭 시 공격
            {
                print("공격 시전");
                Ray ray = new Ray(attackPoint.position, attackPoint.forward);
                Debug.DrawRay(ray.origin, ray.direction * 1.5f, Color.blue);
                RaycastHit hit;
                currentTime = 0;//공격 쿨타임 돌리기
                if (Physics.Raycast(ray, out hit, 1.5f)) //1,5.길이의 짧은 레이(근접 공격)에 보스가 맞으면 보스의 데미지 처리 함수를 불러와 공격 
                {
                    if (hit.collider.CompareTag("Test"))///////////////////////////////보스 태그이름
                    {
                        bossWay.GetComponent<BossScript>().HitEnemy(attackPower);
                        print("보스 공격");
                    }
                }
            }
        }

    }
    private void Move()
    {
        if(isLoading)
            return;
        movingWay = new Vector3(hAxis, 0, vAxis).normalized;
        movingWay = transform.TransformDirection(movingWay);
        if (isAttacked)
        {
            print("hit");
            movingWay = (bossWay.transform.position - gameObject.transform.position).normalized;
            //movingWay = transform.TransformDirection(movingWay);
            playerController.Move(-movingWay * 100 * Time.deltaTime);
            Invoke("isAttackedFalse", 0.5f);
            bossWay.GetComponent<BossScript>().ReturntoMove();
            return;
        }
        playerController.Move(movingWay * playerSpeed * Time.deltaTime);
        anim.SetBool("IsRun", movingWay != Vector3.zero);
    }

    void Gravity()
    {
        jumpVector = new Vector3(0f, -20.0f, 0f);
        playerController.Move(jumpVector * Time.deltaTime);
    }

    void Dodge()
    {
        if (jDown && movingWay != Vector3.zero && !isDodge)
        {
            playerSpeed *= 2;
            isDodge = true;
            
            Invoke("DodgeOut", 0.5f);
        }
    }

    void DodgeOut()
    {
        playerSpeed *= (float)0.5;
        Invoke("DodgeFalse", 10.0f);
    }

    void DodgeFalse()
    {
        isDodge = false;
    }

    void Interaction()
    {
        if (iDown)
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 2.5f))
            {
                if (hit.collider.CompareTag("Door"))
                {
                    clickObject = hit.collider.gameObject;
                    gameManager.Field_Change(clickObject);
                    isLoading = true;
                }

                if (hit.collider.CompareTag("Test"))
                {
                    Destroy(hit.transform.gameObject);
                    for (int i = 0; i < 3; i++)
                    {
                        GameObject.Find("Boss").transform.GetChild(i).gameObject.SetActive(true);
                        boss.GetComponent<BoxCollider>().enabled = true;
                        boss.GetComponent<BossScript>().enabled = true;
                        boss.GetComponent<EnemySkill>().enabled = true;
                    }
                }
            }
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