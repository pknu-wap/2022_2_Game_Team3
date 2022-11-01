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

    public float bossSpeed = 75f;
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
                transform.Translate(direction * bossSpeed * Time.deltaTime);
                Invoke("isRushFalse", 1f);
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

    void isRushFalse()
    {
        isRush = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<TestPlayer>().isAttacked = true;
            isRush = true;
        }
    }

    
}
