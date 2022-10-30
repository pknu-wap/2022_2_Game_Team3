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
    public bool isaddforce = false;
    private bool isJump;
    private bool isDodge;
    private bool isStunned = false;
    public float speed;
    public float gravity;
    public float jumpSpeed;
    private bool jumped = false;

    private Vector3 jumpVector;

    private Vector3 movingWay;

    private Animator anim;

    private Rigidbody rigid;

    private CharacterController cc;

    // Start is called before the first frame update
    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        speed = 6.0f;
        jumpSpeed = 9.8f;
        gravity = 9.8f;
    }

    // Update is called once per frame
    void Update()
    {
        Gravity();
        GetInput();
        Move();
        //Turn();
        Jump();
        //Dodge();
    }

    void GetInput()
    {
        if (isStunned == true)
        {
            return;
        }
        else
        {
            hAxis = Input.GetAxis("Horizontal");
            vAxis = Input.GetAxis("Vertical");
            wDown = Input.GetButton("Walk");
            jDown = Input.GetButtonDown("Jump");
        }
    }

    private void Move()
    {
        movingWay = new Vector3(hAxis, 0, vAxis).normalized;
        if (isaddforce)
        {
            cc.Move(-movingWay * 100 * Time.deltaTime);
            Invoke("addFrocedFalse", 0.5f);
            return;
        }

        if (wDown)
        {
            cc.Move(movingWay * playerSpeed * 0.3f * Time.deltaTime);
            anim.SetBool("IsWalk", wDown);
        }
        else
        {
            cc.Move(movingWay * playerSpeed * Time.deltaTime);
            anim.SetBool("IsRun", movingWay != Vector3.zero);
        }
    }

    void JumpFalse()
    {
        jumped = false;
        isJump = false;
    }

    void Jump()
    {
        if (jumped)
        {
            cc.Move(jumpVector * (float)2.5 * Time.deltaTime);
            Invoke("JumpFalse", 0.5f);
        }

        if (jDown && !isJump)
        {
            jumpVector.y = jumpSpeed;
            //cc.Move(Vector3.up * 35 * Time.deltaTime);
            //cc.Move(jumpVector);
            isJump = true;
            jumped = true;
        }
    }

    void Gravity()
    {
        if (jumped == true)
        {
            return;
        }

        jumpVector = new Vector3(0f, -20.0f, 0f);
        //jumpVector.y -= gravity;

        //movingWay = Vector3.zero;
        //movingWay.y -= gravity;
        cc.Move(jumpVector * Time.deltaTime);
        //movingWay.y = 0;
    }

    /*void Dodge()
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
    } */

    void Turn()
    {
        if (movingWay != Vector3.zero)
        {
            Vector3 relativePos = (transform.position + movingWay) - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 10);
        }
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isJump = false;
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Zone"))
        {
            playerSpeed *= (float)0.5;
            //isStunned = true;
            //anim.SetBool("IsRun", false);
            Debug.Log("속도 느려짐");
        }
    }

    void StunnedOut()
    {
        playerSpeed *= (float)2.0;
        //isStunned = false;
        Debug.Log("실행됨");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Zone"))
        {
            Invoke("StunnedOut", 3f);
        }
    }

    void addFrocedFalse()
    {
        isaddforce = false;
    }
}