using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Dictionary<GameObject, int> roomState; // Room Clear?
    // 플레이어
    public Player player;

    // 시스템 매니저
    public GameObject systemManager;
    

    // 상태 조건
    private bool isGetArtifact = false; // Room Artifact Get?
    private bool isNeedArtifact = false; // if Get Room Artifact Player can Escape

    // 필드 및 스폰포인트.
    public GameObject[] fields;
    public Transform[] spawnPoints;

    public GameObject[] save_Artifacts;
    public GameObject[] maze_Spawn_point;
    public GameObject[] treasure_Spawn_point;
    public GameObject[] room_Artifacts;
    public GameObject[] rooms;
    public GameObject[] treasuresBox;
    
    // 현재 플레이어 방오브젝트
    public GameObject playerInRoomOBJ;

    // 현재 먹은 유물 번호
    public int artifactNum = -1;    // 0 -> Maze Artifact, 1 -> Jump Artifact, 2 -> treasure Artifact, 3 -> Quiz Artifact 

    // 함정들
    // 1. Lava Set parameter
    public GameObject Jump_Map_lava;
    public Vector3 lavaStartPos;

    void Start()
    {
        roomState = new Dictionary<GameObject, int>();
        lavaStartPos = new Vector3(0, -37, 300);
        
        for(int i = 0; i < rooms.Length; i++)
        {
            roomState.Add(rooms[i], 0);
        }

        foreach(KeyValuePair<GameObject, int> pair in roomState)
        {
            Debug.Log(pair.Key.name + "," + pair.Value);
        }
    }

    public void Field_Change(GameObject interactionOBJ)
    {
        Objects OBJcomponent = interactionOBJ.GetComponent<Objects>();
        if(OBJcomponent.inRoom)
        {
            //isNeedArtifact = true;
            playerInRoomOBJ = OBJcomponent.RoomOBJ;
            if(roomState[playerInRoomOBJ] != 0)
            {
                player.isLoading = false;
                Debug.Log("Aready Clear!!");
                return;
            }
            isNeedArtifact = true;
            Transform nextPos = OBJcomponent.NextRoomPosition;
            systemManager.GetComponent<FadeInOut>().FadeFunc();
                
            StartCoroutine(tpPos(nextPos));
        }
        else
        {
            if(!isGetArtifact && !isNeedArtifact)  {    
                Transform nextPos = OBJcomponent.NextRoomPosition;
                systemManager.GetComponent<FadeInOut>().FadeFunc();
                    
                StartCoroutine(tpPos(nextPos));
            }
            else{
                Debug.Log("Can't exit");
                player.isLoading = false;
                return;
            }
            if(artifactNum >= 0) {
                save_Artifacts[artifactNum].SetActive(true);
            }
            StartCoroutine(Current_Save());
        }
    }

    public void Get_Artifact(GameObject artifact)
    {
        Debug.Log("얻은 유물 이름 : " + artifact.name);
        if(artifact.name == room_Artifacts[0].name)
        {
            room_Artifacts[0].SetActive(false);
            Maze_Room_Second_Phase();
            roomState[rooms[0]] = 1;
            artifactNum = 0;
        }
        else if(artifact.name == room_Artifacts[1].name)
        {
            room_Artifacts[1].SetActive(false);
            Jump_Room_Second_Phase();
            roomState[rooms[1]] = 1;
            artifactNum = 1;
        }
        else if(artifact.name == room_Artifacts[2].name)
        {
            room_Artifacts[2].SetActive(false);
            Treasure_Second_Phase();
            roomState[rooms[2]] = 1;
            artifactNum = 2;
        }
        if(isNeedArtifact)
        {
            isNeedArtifact = false;
        }
    }

    // 미로 2페이즈
    public void Maze_Room_Second_Phase()
    {
        for(int i = 0; i < maze_Spawn_point.Length; i++)
        {
            maze_Spawn_point[i].transform.GetChild(0).transform.localPosition = new Vector3(0,0,0);
            maze_Spawn_point[i].SetActive(true);
        }
    }

    // 미로 초기화!
    public void Maze_Room_First_Phase()
    {
        for (int i = 0; i < maze_Spawn_point.Length; i++)
        {
            maze_Spawn_point[i].SetActive(false);
        }
        room_Artifacts[artifactNum].SetActive(true);
        save_Artifacts[artifactNum].SetActive(false);
        artifactNum = -1;
    }

    // 점프맵 2페이즈
    public void Jump_Room_Second_Phase()
    {
        StartCoroutine(LavaON(Jump_Map_lava));
    }

    public void Jump_Room_First_Phase()
    {
        room_Artifacts[artifactNum].SetActive(true);
        save_Artifacts[artifactNum].SetActive(false);
        Jump_Map_lava.transform.localPosition = lavaStartPos;
        artifactNum = -1;
    }

    public void Treasure_Second_Phase()
    {
        for(int i = 0; i < treasure_Spawn_point.Length; i++)
        {
            treasure_Spawn_point[i].transform.GetChild(0).transform.localPosition = new Vector3(0,0,0);
            treasure_Spawn_point[i].SetActive(true);
        }
    }

    public void Treasure_First_Phase()
    {
        for(int i = 0; i < treasure_Spawn_point.Length; i++)
        {
            treasure_Spawn_point[i].SetActive(false);
        }
        for(int i = 0; i < treasuresBox.Length; i++)
        {
            treasuresBox[i].GetComponent<Treasure>().reward.SetActive(false);
            treasuresBox[i].GetComponent<Treasure>().box.SetActive(true);
            treasuresBox[i].gameObject.GetComponent<BoxCollider>().enabled = true;        
        }
        save_Artifacts[artifactNum].SetActive(false);
        artifactNum = -1;
    }

    // 게임 오버
    public void GameOver()
    {
        player.isLoading=true;
        StartCoroutine(GameOverLoadSetting());
    }

    IEnumerator Current_Save()
    {
        yield return new WaitForSeconds(3.0f);
        playerInRoomOBJ.SetActive(false);
    }

    IEnumerator tpPos(Transform nextPos)
    {
        yield return new WaitForSeconds(2.0f);
        Vector3 spawnPos = new Vector3(nextPos.position.x, nextPos.position.y + 1, nextPos.position.z);
        player.transform.position = spawnPos;
    }

    IEnumerator LavaON(GameObject lava)
    {
        yield return new WaitForSeconds(0.125f);
        if(lava.transform.position.y <= 50 && player.live == true)
        {
            lava.transform.Translate(Vector3.up * Time.deltaTime);
            Debug.Log(Jump_Map_lava.transform.position);
            StartCoroutine(LavaON(lava));
        }
    }

    IEnumerator GameOverLoadSetting()
    {
        systemManager.GetComponent<FadeInOut>().GameOverFadeFunc();
        yield return new WaitForSeconds(3.0f);

        player.transform.position = spawnPoints[1].transform.position;
        player.live = true;
        if(artifactNum == 0)
        {
            roomState[rooms[artifactNum]] = 0;
            Maze_Room_First_Phase();
        }
        else if(artifactNum == 1)
        {
            roomState[rooms[artifactNum]] = 0;
            Jump_Room_First_Phase();
        }
        else if(artifactNum == 2)
        {
            roomState[rooms[artifactNum]] = 0;
            Treasure_First_Phase();
        }
    }
}