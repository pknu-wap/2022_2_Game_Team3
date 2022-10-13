using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    private Transform playerTransform;

    private float timer;
    private bool isRush = false;
    private Vector3 heading;
    private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        rush();
    }

    void rush()
    {
        if (!isRush)
        {
            timer = 0;
            heading = playerTransform.transform.position - transform.position;
            float distance = heading.magnitude;
            direction = heading.normalized;
            direction.y = 0;

            if (distance > 30 * Time.deltaTime)
            {
                transform.Translate(direction * (float)100 * Time.deltaTime);
            }
        }
        else
        {
            timer += Time.deltaTime;
            if (timer > 8)
            {
                isRush = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 dir = collision.gameObject.transform.position - transform.position;
            dir.y = 0f;
            dir = (dir.normalized) * 30;
            collision.gameObject.GetComponent<CharacterController>().Move(dir*100);
            Debug.Log("밀치기");
            isRush = true;
        }
    }

    
}
