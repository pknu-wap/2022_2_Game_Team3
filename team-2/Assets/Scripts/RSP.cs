using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RSP : MonoBehaviour
{
    enum RCPNum {
        Rock = 4,
        Scissor = 5,
        Paper = 6,
    };
    [SerializeField] string playerRCP;
    [SerializeField] string randomRCP;

    public GameManager gameManager;
    public SystemManager systemManger;
    public Player player;
    public GameObject[] screen_s;
    public GameObject[] stairs;
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

        screen_s[7].SetActive(false);
        screen_s[8].SetActive(false);
        screen_s[0].SetActive(true);
        // CountDown 3,2,1
        yield return new WaitForSeconds(2.0f);
        screen_s[0].SetActive(false);
        screen_s[1].SetActive(true);
        yield return new WaitForSeconds(1.0f);
        screen_s[1].SetActive(false);
        screen_s[2].SetActive(true);
        yield return new WaitForSeconds(1.0f);
        screen_s[2].SetActive(false);
        screen_s[3].SetActive(true);
        yield return new WaitForSeconds(1.0f);
        screen_s[3].SetActive(false);
        screen_s[randomNum].SetActive(true);
        StartCoroutine(CompareGame(playerRCP, randomRCP));
    }

    IEnumerator CompareGame(string playerRSP, string randomRSP)
    {
        yield return new WaitForSeconds(2.0f);
        if(playerRSP == "Rock")
        {
            if(randomRSP == "Scissor")
            {
                stairs[PlayerWinCount].SetActive(true);
                PlayerWinCount++;
                screen_s[(int)RCPNum.Scissor].SetActive(false);
                screen_s[7].SetActive(true);
            }
            else
            {
                screen_s[(int)RCPNum.Scissor].SetActive(false);
                screen_s[8].SetActive(true);

                for(int i = 0; i < stairs.Length; i++)
                {
                    stairs[i].SetActive(false);
                }
                PlayerWinCount = 0;

                if(player.live)
                {
                    player.live = false;
                    gameManager.GameOver();
                }
            }
        }
        else if(playerRSP == "Scissor")
        {
            if(randomRSP == "Paper")
            {
                stairs[PlayerWinCount].SetActive(true);
                PlayerWinCount++;
                screen_s[(int)RCPNum.Paper].SetActive(false);
                screen_s[7].SetActive(true);
            }
            else
            {
                screen_s[(int)RCPNum.Paper].SetActive(false);
                screen_s[8].SetActive(true);
                
                for(int i = 0; i < stairs.Length; i++)
                {
                    stairs[i].SetActive(false);
                }
                PlayerWinCount = 0;

                if(player.live)
                {
                    player.live = false;
                    gameManager.GameOver();
                }
            }
        }
        else if(playerRSP == "Paper")
        {
            if(randomRSP == "Rock")
            {
                stairs[PlayerWinCount].SetActive(true);
                PlayerWinCount++;
                screen_s[(int)RCPNum.Rock].SetActive(false);
                screen_s[7].SetActive(true);
            }
            else
            {
                screen_s[(int)RCPNum.Rock].SetActive(false);
                screen_s[8].SetActive(true);
                
                for(int i = 0; i < stairs.Length; i++)
                {
                    stairs[i].SetActive(false);
                }
                PlayerWinCount = 0;

                if(player.live)
                {
                    player.live = false;
                    gameManager.GameOver();
                }
            }
        }
        yield return new WaitForSeconds(2.0f);
        isPlay = false;
    }
}
