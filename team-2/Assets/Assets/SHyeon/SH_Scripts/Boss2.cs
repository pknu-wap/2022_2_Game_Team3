using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss2 : MonoBehaviour
{
    public GameObject prefabs; 
    private BoxCollider area;

    private List<GameObject> gameObject = new List<GameObject>();
    
    void Start()
    {
        area = GetComponent<BoxCollider>();
        //area.enabled = false;
    }

    private void Update()
    {
        StartCoroutine("Spawn");
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 basePosition = transform.position;
        Vector3 size = area.size;
        
        float posX = basePosition.x + Random.Range(-size.x/2f, size.x/2f);
        float posZ = basePosition.z + Random.Range(-size.z/2f, size.z/2f);
        
        Vector3 spawnPos = new Vector3(posX, 1, posZ);
        
        return spawnPos;
    }
    
    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(5f);
        GameObject selectedPrefab = prefabs;
        
        Vector3 spawnPos = GetRandomPosition();//랜덤위치함수
        
        GameObject instance = Instantiate(selectedPrefab, spawnPos, Quaternion.identity);
        gameObject.Add(instance);
    }
}