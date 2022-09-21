using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float h, v;
    float playerSpeed = 2;
    float jumpPower = 5;
    bool isJump;
    Vector3 moveingWay;
    Rigidbody rigid;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        isJump = false; 
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        moveingWay = new Vector3(h, 0, v);
        transform.Translate(moveingWay * playerSpeed * Time.deltaTime);
        if (Input.GetButtonDown("Jump") && !isJump)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isJump = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            Debug.Log("´Ù½Ã false·Î");
            isJump = false;
        }
    }
}
