using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    Dictionary<int, string[]> content;
    Dictionary<int, string[]> information_Rooms;

    void Awake()
    {
        content = new Dictionary<int, string[]>();
        information_Rooms = new Dictionary<int, string[]>();
        GenerateContent();
        GenerateInformationRooms();
    }

    void GenerateContent()
    {
        content.Add(0, new string[] {"어서와", "여기는 처음이지?", "나도 너처럼 여기 같인 사람들중 한명이야."});
        content.Add(1, new string[] {"여기서 나갈려면 각 방에 존재하는 유물 4개를 모아와야해.", "방은 미로, 점프맵, 보물찾기, 가위바위보로 구성되어있어.","각 방의 정보는 내가 알고 있어.", "원하는 정보를 알고싶다면 나에게 대화를 걸어!"});
        content.Add(2, new string[] {"어떤 정보를 원하니?"});
    }

    void GenerateInformationRooms()
    {
        information_Rooms.Add(0, new string[] {"왼쪽 첫번째 방이 미로방이야.", "미로에는 알수없는 존재들이 살고있는 것만 알아.", "항상 뒤를 조심하길바래!"});
        information_Rooms.Add(1, new string[] {"왼쪽 두번째 방은 점프맵이야.", "옛날에는 누가 살던 마을이였던거 같은데...", "들어갈때는 마을이지만 나오기 위해서는 점프맵을 통과해야 하나봐.", "떨어지지 않게 조심해!"});
        information_Rooms.Add(2, new string[] {"오른쪽 첫번째 방은 보물찾기 맵이야.", "과거 누가 살았는지 모르겠지만 지금은 폐허가된 모양이더라구.", "보물상자에 뭐가 튀어나올지 모르니 조심해!"});
        information_Rooms.Add(3, new string[] {"오른쪽 두번째 방은 가위바위보 맵이야.", "유물을 획득하기 위해서는 가위바위보를 해서 3번 이겨야하나봐.", "가위바위보에 지거나 무승부가 되면 무슨일이 벌어질지 모르니 조심해!"});
        information_Rooms.Add(4, new string[] {"유물을 다모으게 되면 저기 뒤에 보이는 문을 열수있어.", "저 방안에는 무시무시한 존재가 살고 있는 것 같아..", "아마 저 존재를 쓰러뜨리면 여기서 나갈 수 있지 않을까?"});
    }

    public string GetContent(int id, int contentNum)
    {
        if(contentNum >= content[id].Length)
        {
            return null;
        }
        else 
        {
            return content[id][contentNum];
        }
    }

    public string GetInformation(int roomId, int informationNum)
    {
        if(informationNum >= information_Rooms[roomId].Length)
        {
            return null;
        }
        else
        {
            return information_Rooms[roomId][informationNum];
        }
    }
}
