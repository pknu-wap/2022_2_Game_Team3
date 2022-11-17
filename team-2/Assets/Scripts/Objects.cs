using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour
{
    public Transform NextRoomPosition;
    public bool inRoom;
    public GameObject RoomOBJ;

    public GameObject GetRoomOBJ()
    {
        if(RoomOBJ.activeSelf == true)
        {
            return RoomOBJ;
        }
        else
        {
            return null;
        }
    }
}
