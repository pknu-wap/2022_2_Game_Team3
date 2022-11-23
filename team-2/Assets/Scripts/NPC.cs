using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    Dictionary<int, string[]> content;

    void Awake()
    {
        content = new Dictionary<int, string[]>();
        GenerateContent();
    }

    void GenerateContent()
    {
        content.Add(0, new string[] {"어서와", "여기는 처음이지?"});
        content.Add(1, new string[] {"여기서 나갈려면 유물 4개를 모아와야해.", "각 방의 정보는 내가 알고 있어.", "원하는 정보를 알고싶다면 나에게 대화를 걸어!"});
        content.Add(2, new string[] {"어떤 정보를 원하니?"});
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
}
