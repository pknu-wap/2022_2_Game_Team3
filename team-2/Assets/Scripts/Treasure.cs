using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public GameObject reward;
    public GameObject box;

    public void OpenBox()
    {
        reward.SetActive(true);
        box.SetActive(false);
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
    }
}
