using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RSP : MonoBehaviour
{
    enum RCPNum {
        Paper = 4,
        Scissor = 5,
        Rock = 6,
    };
    [SerializeField] string playerRCP;
    [SerializeField] string randomRCP;

    public GameObject[] screen_s;
    bool isPlay;

    public int PlayerWinCount = 0;

    void Awake()
    {
        PlayerWinCount = 0;
        isPlay = false;
    }

    public void SetPlayerRCP(string RCP)
    {
        playerRCP = RCP;
        if(!isPlay && PlayerWinCount < 3)
        {
            isPlay = true;
            StartCoroutine(RCP_Start());
        }
    }

    IEnumerator RCP_Start()
    {
        int randomNum = Random.Range(4,7);
        randomRCP = ((RCPNum)randomNum).ToString();
        Debug.Log("randomRCP ÀÇ °ª : " + randomRCP);

        yield return new WaitForSeconds(2.0f);
        screen_s[0].SetActive(false);
        screen_s[1].SetActive(true);
        yield return new WaitForSeconds(2.0f);
        screen_s[1].SetActive(false);
        screen_s[2].SetActive(true);
        yield return new WaitForSeconds(2.0f);
        screen_s[2].SetActive(false);
        screen_s[3].SetActive(true);
        yield return new WaitForSeconds(2.0f);
        screen_s[3].SetActive(false);
        screen_s[randomNum].SetActive(true);
    }
}
