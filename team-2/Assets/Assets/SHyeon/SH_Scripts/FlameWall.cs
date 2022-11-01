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

    IEnumerator Flame()
    {
        for (int i = 0; i < 8; i++)
        {
            GameObject.Find("FlameWallGroup").transform.GetChild(i).gameObject.SetActive(true);
        }
        Debug.Log("광역기 생성됨");
        gameObject.GetComponent<BoxCollider>().enabled = true;
        Invoke("FlameFalse", 3f);
        yield return new WaitForSeconds(10f);
        StartCoroutine("Flame");
    }
    void FlameFalse()
    {
        for (int i = 0; i < 8; i++)
        {
            GameObject.Find("FlameWallGroup").transform.GetChild(i).gameObject.SetActive(false);
        }
        gameObject.GetComponent<BoxCollider>().enabled = false;
        Debug.Log("광역기 사라짐");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<TestPlayer>().isAttacked = true;
            Debug.Log("광역기 접촉함");
        }
    }
}
