using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomPosition : MonoBehaviour
{
    private BoxCollider area;
    void Start()
    {
        area = GameObject.Find("RandomPosition").GetComponent<BoxCollider>();
        StartCoroutine("Spawn");
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 basePosition = area.transform.position;
        Vector3 size = area.size;
        
        float posX = basePosition.x + Random.Range(-size.x/2f, size.x/2f);
        float posZ = basePosition.z + Random.Range(-size.z/2f, size.z/2f);
        
        Vector3 spawnPos = new Vector3(posX, 0.5f, posZ);
        
        return spawnPos;
    }
    
    IEnumerator Spawn()
    {
        Vector3 spawnPos = GetRandomPosition();

        transform.position = spawnPos;
        yield return new WaitForSeconds(15f);
        StartCoroutine("Spawn");
    }
}
