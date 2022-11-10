using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyFSM : MonoBehaviour
{
    NavMeshAgent smith;
    //public float moveSpeed = 5f;
    public float attackDistance = 2f;//���� ��Ÿ�
    public float findDistance = 8f;//�÷��̾� �ν� �Ÿ�
    public float stopDistance = 100;//Idle���·� ���ƿ��� �Ÿ� 
    public int attackPower = 3;//���ݷ�
    
    Transform player;//�÷��̾��� ��ǥ ��������
    CharacterController cc;//ĳ���� ��Ʈ�ѷ�
    // Start is called before the first frame update
    public enum EnemyState{
        Idle,//�⺻ ����(������ ����)
        Move,//�÷��̾ �����ϱ� ���� �����̴� ����
        Attack,//�÷��̾ �����Ϸ��� ����
        Attacking//���ݸ���� ó���ϴ� ���� 2 1.3 44
    }
    float currentTime = 0;//���ݼӵ��� ���̴� �ð�����
    float attackDelay = 2f;//���ݰ� ����
    public EnemyState m_State;//���� ����
    void Start()
    {
        m_State = EnemyState.Idle;
        cc = GetComponent<CharacterController>();
        player = GameObject.Find("Test_Player").transform;//�÷��̾��� ��ġ�� ������
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
        }
    }

    void Idle()
    {
        if(Vector3.Distance(transform.position, player.position) < findDistance)//�÷��̾ �ν� �Ÿ� ���� ������ �����̴� ���·� ��ȯ
        {
            m_State = EnemyState.Move;
            print("Idle -> Move");
        }
    }

    void Move()
    {
        float Distance = Vector3.Distance(transform.position, player.position);//����ȭ

        if (Distance > attackDistance && Distance < stopDistance)//�÷��̾ ���� ��Ÿ����� �ְ�, ���� ������������ Ż������ �ʾ����� ��ã��� �÷��̾� ã�ư���
        {
            //Vector3 dir = (player.position - transform.position).normalized;

            //cc.Move(dir * moveSpeed * Time.deltaTime);

            smith.isStopped = true;

            smith.ResetPath();

            smith.stoppingDistance = attackDistance;

            smith.destination = player.position;
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
        if(Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            currentTime += Time.deltaTime;
            if(currentTime > attackDelay)
            {
                print("Attack");
                player.GetComponent<Player2>().DamageAction(attackPower);//player��ũ��Ʈ�� �ִ� �÷��̾� ���� �Լ��� ������ ����
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

}
