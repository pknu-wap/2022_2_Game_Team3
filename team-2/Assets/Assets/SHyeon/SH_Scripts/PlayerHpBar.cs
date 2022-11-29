using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBar : MonoBehaviour
{
    private BossPlayer player;
    [SerializeField] private Slider hpBar;
    private float maxHp;
    private float curHp;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<BossPlayer>();
        maxHp = player.hp;
        curHp = player.hp;
        hpBar.value = (float)curHp / (float)maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        curHp = player.hp;
        HandleHp();
    }

    private void HandleHp()
    {
        hpBar.value = Mathf.Lerp(hpBar.value, curHp / maxHp, Time.deltaTime);
    }
}
