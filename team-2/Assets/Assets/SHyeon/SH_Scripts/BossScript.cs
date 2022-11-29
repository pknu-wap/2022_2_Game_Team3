using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BossScript : MonoBehaviour
{
    Animator anim;
    public NavMeshAgent smith;
    private Transform playerTransform;
    private Vector3 heading;
    private Vector3 direction;

    BossPlayer PlayerAttacked;

    public int hp = 9;//보스 체력
    public float bossSpeed = 75f;
    public float attackDistance = 20f;//���� ��Ÿ�
    public float findDistance = 50f;//�÷��̾� �ν� �Ÿ�
    public float stopDistance = 100;//Idle���·� ���ƿ��� �Ÿ� 
    public int attackPower = 3;
    
    Transform player;//�÷��̾��� ��ǥ ��������
    public enum EnemyState{
        Idle,
        Move,//�÷��̾ �����ϱ� ���� �����̴� ����
        Attack,//�÷��̾ �����Ϸ��� ����
        Attacking,
        Rush,//���ݸ���� ó���ϴ� ���� 2 1.3 44
        Damaged,/////////////////////////////////////////////데미지 처리상태
        Die /////////////////////////죽음 처리 상태

    }
    private float rushCoolTime;
    float currentTime;//���ݼӵ��� ���̴� �ð�����
    float attackDelay = 2f;//���ݰ� ����
    public EnemyState m_State;
    
    void Start()
    {
        anim = transform.GetComponentInChildren<Animator>();
        PlayerAttacked = GameObject.Find("Player").GetComponent<BossPlayer>();
        playerTransform = GameObject.Find("Player").transform;
        m_State = EnemyState.Idle;
        smith = GetComponent<NavMeshAgent>();//���ʹ��� �׺�޽ÿ�����Ʈ ��������

    }

    // Update is called once per frame
    void Update()
    {
        switch (m_State)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                DeafultAttack();
                break;
            case EnemyState.Attacking:
                Attacking();
                break;
            case EnemyState.Rush:
                Rush();
                break;
            case EnemyState.Damaged://///////////////////////////////////
                break;
            case EnemyState.Die:///////////////////////
                break;
        }
    }

    void Idle()
    {
        if (transform.GetComponent<BoxCollider>().enabled == true)
            m_State = EnemyState.Move;
        //     if(Vector3.Distance(transform.position, playerTransform.position) < findDistance)//�÷��̾ �ν� �Ÿ� ���� ������ �����̴� ���·� ��ȯ
        // {
        //     m_State = EnemyState.Move;
        //     print("Idle -> Move");
        // }
    }
    void Move()
    {
        currentTime += Time.deltaTime;///////////////////////////////////********
        rushCoolTime += Time.deltaTime;
        if (rushCoolTime > 10)
        {
            m_State = EnemyState.Rush;
        }
        float Distance = Vector3.Distance(transform.position, playerTransform.position);//����ȭ

        if (Distance > attackDistance)//�÷��̾ ���� ��Ÿ����� �ְ�, ���� ������������ Ż������ �ʾ����� ��ã��� �÷��̾� ã�ư���
        {
            smith.isStopped = true;

            smith.ResetPath();

            smith.stoppingDistance = attackDistance;

            smith.destination = playerTransform.position;
        }
        else//�÷��̾ ���ݻ�Ÿ� ���϶�(else if��?)
        {
            m_State = EnemyState.Attack;
            print("Move -> Attack");
            ///////////////////////////////currentTime = attackDelay;//���� ���� ��ȯ �� �ٷ� ������ �� �ְ� ����
        }
    }
    
    void DeafultAttack()//���� ���� ���� �÷��̾ ���� ��Ÿ� ���̸�currentTime�� ���� attackDelay���� ũ�� ����, ���� �� ���ݸ�� ���·� �ѱ�
    {
        currentTime += Time.deltaTime;
        if(currentTime > attackDelay)
        {
            print("Attack");
            playerTransform.GetComponent<BossPlayer>().DamageAction(attackPower);//수정 필요함 player��ũ��Ʈ�� �ִ� �÷��̾� ���� �Լ��� ������ ����

            currentTime = 0;//���� �� �����̸� ���� 
            m_State = EnemyState.Attacking;
            anim.SetTrigger("MoveToAttack");
        }
        {
            m_State = EnemyState.Move;
            anim.SetTrigger("AttackToMove");
            print("Attack -> Move");
        }
    }
     void Rush()
     {
        smith.speed = 3000;
        //smith.angularSpeed = 5000;
        smith.acceleration = 1000;
        smith.stoppingDistance = 0;

        //smith.stoppingDistance = attackDistance;

        smith.destination = playerTransform.position;
        print("Rush");
        //Invoke("IsRushFalse", 1f);
       
        /*heading = playerTransform.transform.position - transform.position;
         float distance = heading.magnitude;
         direction = heading.normalized;
         direction.y = 0;*/

        /*if (distance > 30 * Time.deltaTime)
         {
             print("Rush");
             transform.Translate(direction * bossSpeed * Time.deltaTime);
             
         }
         else
         {
             
         }*/
    }
     
    void Attacking()//���� ��� ���¿��� ���� �Լ�
    {
        StartCoroutine(Attackmotion());//�ڷ�ƾ�Լ��� �̿�
    }

    IEnumerator Attackmotion()//���� ��� �� ó��
    {
        yield return new WaitForSeconds(0.5f);//0.5�� �Ŀ� ���¸� attack���� ����->0.5�� ���� Attacking ���°� �����Ǹ� �� ������ ���ʹ̰� ������ ����
        m_State = EnemyState.Attack;          //
        StopCoroutine(Attackmotion());        //Attacking ���¿��� Attacking �Լ��� ���� �� ����ʿ� ���� �ڷ�ƾ �Լ��� ���� �� �Լ��� �̿��� ���� �ڷ�ƾ �Լ� ����
    }

    /*void IsRushFalse()
    {
        print("EnemyState : Move");
        m_State = EnemyState.Move;
        //anim.SetTrigger("MoveToIdle");
        anim.SetTrigger("AttackToMove");
    }*/

    public void HitEnemy(int hitPower)//플레이어에 의해 호출될 보스에게 데미지를 입히는 함수
    {
        hp -= hitPower;

        if (hp > 0)
        {
            m_State = EnemyState.Damaged;
            print("->Damaged");
            Damaged();
        }
        else
        {
            m_State = EnemyState.Die;
            print("->Die");
            Die();
        }
    }

    void Damaged()//데미지를 입는 동안의 처리 
    {
        StartCoroutine(DamageProcess());

    }

    IEnumerator DamageProcess()//데미지 상태 처리용 코루틴 함수 0.1초 보스가 멈추게 함(공격 받을 시)
    {
        print("보스의 남은 체력" + hp);
        yield return new WaitForSeconds(0.1f);

        m_State = EnemyState.Move;
        print("Damaged -> Move");
    }

    void Die()//보스의 죽음을 처리하는 함수
    {
        StopAllCoroutines();
        anim.SetTrigger("Die");
        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()//보스 죽음 처리용 코루틴 함수 2초 후 보스가 스테이지에서 사라진다. 
    {
        yield return new WaitForSeconds(2f);
        print("Die");
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameObject.Find("Canvas").transform.GetChild(1).gameObject.SetActive(true);
    }

    public void ReturntoMove()
    {
        print("EnemyState : Move");
        rushCoolTime = 0;
        smith.speed = 30;
        smith.angularSpeed = 120;
        smith.acceleration = 8;
        m_State = EnemyState.Move;////////////////////////////////////////////////////////
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("OnCollisionEnter");
        if (collision.gameObject.CompareTag("Player"))
        {
            print("CompareTag Player");
            if (m_State == EnemyState.Rush)
                PlayerAttacked.isAttacked = true;
            /*if (m_State == EnemyState.Rush)
            {
               
            }*/
           
        }
    }
}
