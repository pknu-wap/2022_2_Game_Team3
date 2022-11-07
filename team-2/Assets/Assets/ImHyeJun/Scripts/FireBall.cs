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
        player = GameObject.Find("Test_Player").transform;
        rigid = GetComponent<Rigidbody>();
        rigid.AddForce((player.position - transform.position).normalized * 3f, ForceMode.Impulse);//화염구가 플레이어 방향으로 해당 힘만큼 날아간다.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.transform.GetComponent<Player2>().DamageAction(9);//플레이어의 체력을 감소시킨다.
        Destroy(gameObject);//자신을 삭제
    }
}
