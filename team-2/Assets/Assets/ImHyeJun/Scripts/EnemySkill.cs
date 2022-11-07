using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkill : MonoBehaviour
{
    public GameObject fireball; //화염구 오브젝트 받아오기
    public Transform throwpoint;//플레이어 당겨오기 스킬 쿨타임용 
    float grep_currentTime;//화염구 스킬 쿨타임용
    float fireball_currentTIme = 5f;
    float grepTime = 5f;//플레이어 당겨오기 스킬 쿨타임
    float fireTIme = 5f;//화염구 발사 쿨타임
    //CharacterController playercc;
    Transform playerpos;
    EnemyFSM.EnemyState enemyState;
    // Start is called before the first frame update
    void Start()
    {
        playerpos = GameObject.Find("Test_Player").transform;
        //playercc = playerpos.GetComponent<CharacterController>();
        enemyState = GameObject.Find("Enemy").transform.GetComponent<EnemyFSM>().m_State;
    }

    // Update is called once per frame
    void Update()
    {
        grep_currentTime += Time.deltaTime;
        fireball_currentTIme += Time.deltaTime;
        if (grep_currentTime > grepTime)//스킬 쿨타임이 돌면 플레이어 끌어당기기 위해 플레이어 상태를 스턴으로 만듬
        {
            playerpos.GetComponent<Player2>().p_State = Player2.playState.Stun;
            StartCoroutine(grepingend());
            /*Vector3 way = (transform.position - playerpos.position).normalized;
            playercc.Move(way * 2 *Time.deltaTime);*/
            grep_currentTime = 0;
        }
        /*if (enemyState == EnemyFSM.EnemyState.Move)
        {*/
            if (fireball_currentTIme > fireTIme)//스킬 쿨타임이 돌면 플레이어를 향해 화염구를 던짐
            {
                Instantiate(fireball, throwpoint);
                fireball_currentTIme = 0;
            }
        //}
    }
    IEnumerator grepingend()//공격 모션 중 처리
    {
        yield return new WaitForSeconds(1f);//0.5초 후에 상태를 attack으로 만듬->0.5초 동안 Attacking 상태가 유지되며 그 동안은 에너미가 가만히 있음
        playerpos.GetComponent<Player2>().p_State = Player2.playState.Normal;//
    }
}
