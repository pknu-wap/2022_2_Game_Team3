using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public Transform player;
    Rigidbody rigid;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        rigid = GetComponent<Rigidbody>();
        rigid.AddForce((player.position - transform.position).normalized * 3f, ForceMode.Impulse);//ȭ������ �÷��̾� �������� �ش� ����ŭ ���ư���.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.transform.GetComponent<BossPlayer>().DamageAction(9);//�÷��̾��� ü���� ���ҽ�Ų��.
        Destroy(gameObject);//�ڽ��� ����
    }
}
