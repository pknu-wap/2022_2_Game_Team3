using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 플레이어
    public Player player;

    // 시스템 매니저
    public GameObject systemManager;

    // 필드 및 스폰포인트.
    public GameObject[] fields;
    public Transform[] spawnPoints;

    public GameObject[] save_Artifacts;
    public GameObject[] maze_Spawn_point;
    public GameObject[] room_Artifacts;

    public void Field_Change(GameObject interactionOBJ)
    {
        Transform nextPos = interactionOBJ.GetComponent<Objects>().NextRoomPosition;

        systemManager.GetComponent<FadeInOut>().FadeFunc();

        StartCoroutine(tpPos(nextPos));
    }

    public void Get_Artifact(GameObject artifact)
    {
        Debug.Log("얻은 유물 이름 : " + artifact.name);
        if(artifact.name == room_Artifacts[0].name)
        {
            room_Artifacts[0].SetActive(false);
        }
    }

    IEnumerator tpPos(Transform nextPos)
    {
        yield return new WaitForSeconds(2.0f);
        Vector3 spawnPos = new Vector3(nextPos.position.x, nextPos.position.y + 1, nextPos.position.z);
        player.transform.position = spawnPos;
    }
}