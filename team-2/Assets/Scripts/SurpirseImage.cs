using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurpirseImage : MonoBehaviour
{
    void Awake()
    {
        StartCoroutine(FadeInStartSurprise());
    }

    IEnumerator FadeInStartSurprise()
    {
        float fadetime = 1.0f;
        while(fadetime >= 0f)
        {
            fadetime -= 0.01f;
            yield return new WaitForSeconds(0.03f);
            this.GetComponent<Image>().color = new Color(255, 255, 255, fadetime);
        }

        StartCoroutine(SetReset());
    }

    IEnumerator SetReset()
    {
        this.gameObject.SetActive(false);
        this.GetComponent<Image>().color = new Color(255, 255, 255, 1.0f);
        yield return 0;
    }

}
