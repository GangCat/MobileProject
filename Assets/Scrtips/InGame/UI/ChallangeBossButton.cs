using DI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChallangeBossButton : DIMono
{
    [Inject]
    PlayData playData;

    public Button btn;
    public TMP_Text textKillCount;
    public int btnAppearCnt;

    int curKillCount;
    ChallangeToBossStage enterBossStage = new ChallangeToBossStage();

    public override void Init()
    {
        btn.onClick.AddListener(
            () =>
            {
                EventBus.Publish(enterBossStage);
            });
        btn.interactable = false;
        textKillCount.text = string.Format("{0} / {1}", curKillCount, btnAppearCnt);
    }

    private void Update()
    {
        if (playData.isBossStage)
        {
            btn.gameObject.SetActive(false);
            textKillCount.gameObject.SetActive(false);
            return;
        }
        if(curKillCount != playData.currentKilledEnemyCount)
        {
            curKillCount = playData.currentKilledEnemyCount;

            if(curKillCount >= btnAppearCnt)
            {
                btn.interactable = true;
                textKillCount.gameObject.SetActive(false);
            }
            else
            {
                btn.interactable = false;
                textKillCount.gameObject.SetActive(true);
                textKillCount.text = string.Format("{0} / {1}", curKillCount, btnAppearCnt);
            }
            //btn.interactable = curKillCount >= btnAppearCnt;

            //textKillCount.text = string.Format("{0} / {1}", curKillCount, btnAppearCnt);
        }
    }
}
