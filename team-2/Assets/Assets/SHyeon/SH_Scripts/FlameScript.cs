using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameScript : MonoBehaviour
{
    public bool floorState = false;
    public float flameSpeed = 100.0f;
    void Update()
    {
        if (floorState == true)
        {
            transform.localScale += new Vector3(0, flameSpeed, 0);
        }
    }
}