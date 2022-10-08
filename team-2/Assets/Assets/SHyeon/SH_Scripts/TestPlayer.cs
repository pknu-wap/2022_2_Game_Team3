using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    public float playerSpeed;
    private float hAxis, vAxis;
    private bool wDown;
    private bool jDown;
    
    private bool isJump;
    private bool isDodge;
    private bool isStunned = false;

    private Vector3 movingWay;

    private Animator anim;

    private Rigidbody rigid;
    // Start is called before the first frame update
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        //Turn();
        Jump();
        Dodge();
    }
    
    void GetInput()
    {
        if (isStunned == true)
        {
            return;
        }
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButton("Jump");
    }
    
    private void Move()
    {
            movingWay = new Vector3(hAxis, 0, vAxis).normalized;
            
            if(wDown)
                transform.Translate(movingWay * playerSpeed * 0.3f * Time.deltaTime);
            else
                transform.Translate(movingWay * playerSpeed * Time.deltaTime);

            anim.SetBool("IsRun", movingWay != Vector3.zero);
            anim.SetBool("IsWalk", wDown);
    }

    void Jump()
    {
        if (jDown && movingWay == Vector3.zero && !isJump && !isDodge)
        {
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
            isJump = true;
        }
    }
    
    void Dodge()
    {
        if (jDown && movingWay != Vector3.zero &&!isJump && !isDodge)
        {
            playerSpeed *= 2;
            isDodge = true;
            
            Invoke("DodgeOut", 3.0f);
        }
    }

    void DodgeOut()
    {
        playerSpeed *= (float)0.5;
        isDodge = false;
    }

    void Turn()
    {
        if (movingWay != Vector3.zero)
        {
            Vector3 relativePos = (transform.position + movingWay) - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime*10);   
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isJump = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Zone"))
        {
            playerSpeed = 0;
            isStunned = true;
            Debug.Log("속도 느려짐");
        }
    }

    void StunnedOut()
    {
        playerSpeed = 15;
        isStunned = false;
        Debug.Log("실행됨");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Zone"))
        {
            Invoke("StunnedOut", 3f);
        }
    }
}
