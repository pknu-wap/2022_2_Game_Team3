using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyFSM : MonoBehaviour
{
    private NavMeshAgent smith;
    //public float moveSpeed = 5f;
    public float attackDistance = 2f;//공격 사거리
    public float findDistance = 8f;//플레이어 인식 거리
    public float stopDistance = 100;//Idle상태로 돌아오는 거리 
    public int attackPower = 3;//공격력
    
    Transform player;//플레이어의 좌표 참조변수

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, findDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

    }

    public enum EnemyState{
        Idle,//기본 상태(가만히 있음)
        Move,//플레이어를 공격하기 위해 움직이는 상태
        Attack,//플레이어를 공격하려는 상태
        Attacking//공격모션을 처리하는 상태 2 1.3 44
    }
    float currentTime = 0;//공격속도에 쓰이는 시간변수
    float attackDelay = 2f;//공격간 간격
    public EnemyState m_State;//적의 상태
    void Awake()
    {
        m_State = EnemyState.Idle;
        player = GameObject.Find("Test_Player").transform;//플레이어의 위치를 가져옴
        smith = GetComponent<NavMeshAgent>();//에너미의 네브메시에이전트 가져오기
        smith.speed = 10.0f;
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
                Attack();
                break;
            case EnemyState.Attacking:
                Attacking();
                break;
        }
    }

    void Idle()
    {
        if(Vector3.Distance(transform.position, player.position) <= findDistance)//플레이어가 인식 거리 내에 들어오면 움직이는 상태로 전환
        {
            m_State = EnemyState.Move;
            Debug.Log("Idle -> Move");
        }
    }

    void Move()
    {
        float Distance = Vector3.Distance(transform.position, player.position);//최적화

        if (Distance > attackDistance && Distance <= stopDistance)//플레이어가 공격 사거리보다 멀고, 현재 스테이지에서 탈출하지 않았을때 길찾기로 플레이어 찾아가기
        {
            //Vector3 dir = (player.position - transform.position).normalized;

            //cc.Move(dir * moveSpeed * Time.deltaTime);

            //smith.isStopped = true;

            //smith.ResetPath();

            //smith.stoppingDistance = attackDistance;

            smith.SetDestination(player.position);
        }
        else if (Distance > stopDistance)// 플레이어가 스테이지를 탈출했을때 Idle상태로 전환
        {
            m_State = EnemyState.Idle;
            Debug.Log("Move -> Idle");
        }
        else if(Distance <= attackDistance)//플레이어가 공격사거리 안일때(else if로?)
        {
            m_State = EnemyState.Attack;
            Debug.Log("Move -> Attack");
            currentTime = attackDelay;//공격 상태 전환 시 바로 공격할 수 있게 만듬
        }

    }

    void Attack()//공격 가능 상태 플레이어가 공격 사거리 안이면currentTime을 보고 attackDelay보다 크면 공격, 공격 후 공격모션 상태로 넘김
    {
        if(Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            currentTime += Time.deltaTime;
            if(currentTime > attackDelay)
            {
                Debug.Log("Attack");
                //player.GetComponent<Player2>().DamageAction(attackPower);//player스크립트에 있는 플레이어 피해 함수를 가져와 실행
                currentTime = 0;//공격 간 딜레이를 위해 0으로 
                m_State = EnemyState.Attacking;
            }
            
        }
        else//플레이어가 공격 사거리에서 벗어나면 Move상태로 전환
        {
            m_State = EnemyState.Move;
            Debug.Log("Attack -> Move");
        }
    }

    void Attacking()//공격 모션 상태에서 사용될 함수
    {
        StartCoroutine(Attackmotion());//코루틴함수를 이용
    }

    IEnumerator Attackmotion()//공격 모션 중 처리
    {
        yield return new WaitForSeconds(0.5f);//0.5초 후에 상태를 attack으로 만듬->0.5초 동안 Attacking 상태가 유지되며 그 동안은 에너미가 가만히 있음
        m_State = EnemyState.Attack;          //
        StopCoroutine(Attackmotion());        //Attacking 상태에서 Attacking 함수가 여러 번 시행됨에 따라 코루틴 함수가 쌓임 이 함수를 이용해 쌓인 코루틴 함수 제거
    }

}
