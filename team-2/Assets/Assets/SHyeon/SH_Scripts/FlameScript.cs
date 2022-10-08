using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameScript : MonoBehaviour
{
    public bool floorState = false;
    void Update()
    {
        if (floorState == true)
        {
            transform.localScale += new Vector3(0, 0.01f, 0);
            Debug.Log("용암 차오름");   
        }
    }
}