using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkill : MonoBehaviour
{
    public GameObject fireball; //ȭ���� ������Ʈ �޾ƿ���
    public Transform throwpoint;//�÷��̾� ��ܿ��� ��ų ��Ÿ�ӿ� 
    float grep_currentTime;//ȭ���� ��ų ��Ÿ�ӿ�
    float fireball_currentTIme = 5f;
    float grepTime = 5f;//�÷��̾� ��ܿ��� ��ų ��Ÿ��
    float fireTIme = 5f;//ȭ���� �߻� ��Ÿ��
    //CharacterController playercc;
    Transform playerpos;
    BossScript.EnemyState enemyState;
    // Start is called before the first frame update
    void Start()
    {
        playerpos = GameObject.Find("Test_Player").transform;
        //playercc = playerpos.GetComponent<CharacterController>();
        enemyState = GameObject.Find("Boss").transform.GetComponent<BossScript>().m_State;
    }

    // Update is called once per frame
    void Update()
    {
        grep_currentTime += Time.deltaTime;
        fireball_currentTIme += Time.deltaTime;
        if (grep_currentTime > grepTime)//��ų ��Ÿ���� ���� �÷��̾� ������� ���� �÷��̾� ���¸� �������� ����
        {
            playerpos.GetComponent<BossPlayer>().p_State = BossPlayer.playState.Stun;
            StartCoroutine(grepingend());
            /*Vector3 way = (transform.position - playerpos.position).normalized;
            playercc.Move(way * 2 *Time.deltaTime);*/
            grep_currentTime = 0;
        }
        /*if (enemyState == EnemyFSM.EnemyState.Move)
        {*/
            if (fireball_currentTIme > fireTIme)//��ų ��Ÿ���� ���� �÷��̾ ���� ȭ������ ����
            {
                Instantiate(fireball, throwpoint);
                fireball_currentTIme = 0;
            }
        //}
    }
    IEnumerator grepingend()//���� ��� �� ó��
    {
        yield return new WaitForSeconds(1f);//0.5�� �Ŀ� ���¸� attack���� ����->0.5�� ���� Attacking ���°� �����Ǹ� �� ������ ���ʹ̰� ������ ����
        playerpos.GetComponent<BossPlayer>().p_State = BossPlayer.playState.Normal;//
    }
}
