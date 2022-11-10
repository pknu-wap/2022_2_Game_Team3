using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BossScript : MonoBehaviour
{
    NavMeshAgent smith;
    private Transform playerTransform;
    private float timer;
    private Vector3 heading;
    private Vector3 direction;

    public float bossSpeed = 75f;
    
    //혜준 코드
    public float attackDistance = 2f;//���� ��Ÿ�
    public float findDistance = 8f;//�÷��̾� �ν� �Ÿ�
    public float stopDistance = 100;//Idle���·� ���ƿ��� �Ÿ� 
    public int attackPower = 3;
    
    Transform player;//�÷��̾��� ��ǥ ��������
    public enum EnemyState{
        Idle,//�⺻ ����(������ ����)
        Move,//�÷��̾ �����ϱ� ���� �����̴� ����
        Attack,//�÷��̾ �����Ϸ��� ����
        Attacking,
        Rush//���ݸ���� ó���ϴ� ���� 2 1.3 44
    }
    float currentTime;//���ݼӵ��� ���̴� �ð�����
    float attackDelay = 2f;//���ݰ� ����
    public EnemyState m_State;
    
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
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
        }
    }
    
    void Idle()
    {
        if(Vector3.Distance(transform.position, playerTransform.position) < findDistance)//�÷��̾ �ν� �Ÿ� ���� ������ �����̴� ���·� ��ȯ
        {
            m_State = EnemyState.Move;
            print("Idle -> Move");
        }
    }
    
    void Move()
    {
        float Distance = Vector3.Distance(transform.position, playerTransform.position);//����ȭ

        if (Distance > attackDistance && Distance < stopDistance)//�÷��̾ ���� ��Ÿ����� �ְ�, ���� ������������ Ż������ �ʾ����� ��ã��� �÷��̾� ã�ư���
        {
            smith.isStopped = true;

            smith.ResetPath();

            smith.stoppingDistance = attackDistance;

            smith.destination = playerTransform.position;
        }
        else if (Distance > stopDistance)// �÷��̾ ���������� Ż�������� Idle���·� ��ȯ
        {
            m_State = EnemyState.Move;
            print("Move -> Idle");
        }
        else//�÷��̾ ���ݻ�Ÿ� ���϶�(else if��?)
        {
            m_State = EnemyState.Attack;
            print("Move -> Attack");
            currentTime = attackDelay;//���� ���� ��ȯ �� �ٷ� ������ �� �ְ� ����
        }

    }
    
    void DeafultAttack()//���� ���� ���� �÷��̾ ���� ��Ÿ� ���̸�currentTime�� ���� attackDelay���� ũ�� ����, ���� �� ���ݸ�� ���·� �ѱ�
    {
        if(Vector3.Distance(transform.position, playerTransform.position) < attackDistance)
        {
            currentTime += Time.deltaTime;
            if(currentTime > attackDelay)
            {
                print("Attack");
                playerTransform.GetComponent<Player2>().DamageAction(attackPower);//수정 필요함 player��ũ��Ʈ�� �ִ� �÷��̾� ���� �Լ��� ������ ����
                currentTime = 0;//���� �� �����̸� ���� 0���� 
                m_State = EnemyState.Attacking;
            }
            
        }
        else//�÷��̾ ���� ��Ÿ����� ����� Move���·� ��ȯ
        {
            m_State = EnemyState.Move;
            print("Attack -> Move");
        }
    }
    void Rush()
    {
        timer = 0;
        heading = playerTransform.transform.position - transform.position;
        float distance = heading.magnitude;
        direction = heading.normalized;
        direction.y = 0;

        if (distance > 30 * Time.deltaTime)
        {
            transform.Translate(direction * bossSpeed * Time.deltaTime);
            Invoke("IsRushFalse", 1f);
        }
        else
        {
            timer += Time.deltaTime;
            if (timer > 30)
            {
                m_State = EnemyState.Rush;
            }
        }
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

    void IsRushFalse()
    {
        m_State = EnemyState.Idle;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<TestPlayer>().isAttacked = true;
            m_State = EnemyState.Idle;
        }
    }
}
