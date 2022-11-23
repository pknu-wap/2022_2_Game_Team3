using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cammove : MonoBehaviour
{
    public Player player;
    float rotSpeed;
    float currentRot;
    
    // Start is called before the first frame update
    void Start()
    {
        rotSpeed = 3.0f;
        currentRot = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(!player.isLoading) {
            float rotX = player.systemManager.isAction ? 0 : Input.GetAxis("Mouse Y") * rotSpeed;
            float rotY = player.systemManager.isAction ? 0 : Input.GetAxis("Mouse X") * rotSpeed;

            currentRot -= rotX;

            currentRot = Mathf.Clamp(currentRot, -60f, 60f);


            player.transform.localRotation *= Quaternion.Euler(0, rotY, 0);

            transform.localEulerAngles = new Vector3(currentRot, 0f, 0f);
        }
    }
}
