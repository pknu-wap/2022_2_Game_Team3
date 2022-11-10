using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    public UnityEvent onPlayerDead;
    public int speed;
    public int jumpSpeed;
    private bool isJump = false;
    Rigidbody rigid;
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }
    void Update()
    {
        move();
    }
    void move()
    {
        if (Input.GetButtonDown("Jump") && !isJump)
        {
            rigid.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            isJump = true;
        }
        Vector3 movingWay = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        transform.Translate(movingWay * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Floor"))
        {
            isJump = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Flame"))
        {
            onPlayerDead.Invoke();
            Debug.Log("Dead");
            SceneManager.LoadScene("Room2");
        }
    }
}
