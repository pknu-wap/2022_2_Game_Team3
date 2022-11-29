using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Image yellowScreen;
    
    // Start is called before the first frame update
    void Start()
    {
        playerpos = GameObject.Find("Player").transform;
        //playercc = playerpos.GetComponent<CharacterController>();
        enemyState = GameObject.Find("Boss").transform.GetComponent<BossScript>().m_State;
    }

    // Update is called once per frame
    void Update()
    {
        grep_currentTime += Time.deltaTime;
        fireball_currentTIme += Time.deltaTime;
        if ((grep_currentTime > grepTime) && (enemyState != BossScript.EnemyState.Attack))//��ų ��Ÿ���� ���� �÷��̾� ������� ���� �÷��̾� ���¸� �������� ����
        {
            playerpos.GetComponent<BossPlayer>().p_State = BossPlayer.playState.Stun;
            StartCoroutine(shpwyellowScreen());
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
    IEnumerator shpwyellowScreen()//���� ��� �� ó��
    {
        yellowScreen.color = new Color(255, 220, 0, UnityEngine.Random.Range(0.2f, 0.3f));
        yield return new WaitForSeconds(1f);//0.5�� �Ŀ� ���¸� attack���� ����->0.5�� ���� Attacking ���°� �����Ǹ� �� ������ ���ʹ̰� ������ ����
        yellowScreen.color = Color.clear;
    }

}
