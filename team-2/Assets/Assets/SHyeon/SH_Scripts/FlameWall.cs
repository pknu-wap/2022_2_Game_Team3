using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameWall : MonoBehaviour
{
    void Start()
    {
        StartCoroutine("Flame");
    }

    void FlameFalse()
    {
        for (int i = 0; i < 8; i++)
        {
            GameObject.Find("FlameWallGroup").transform.GetChild(i).gameObject.SetActive(false);
        }
        gameObject.GetComponent<BoxCollider>().enabled = false;

        Debug.Log("불기둥 사라짐");
    }

    IEnumerator Flame()
    {
        for (int i = 0; i < 8; i++)
        {
            
            GameObject.Find("FlameWallGroup").transform.GetChild(i).gameObject.SetActive(true);
        }

        Debug.Log("불기동 생성됨");
        gameObject.GetComponent<BoxCollider>().enabled = true;
        Invoke("FlameFalse", 3f);
        yield return new WaitForSeconds(10f);
        StartCoroutine("Flame");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<TestPlayer>().isaddforce = true;
            Debug.Log("용암 밀치기");
        }
    }
}
