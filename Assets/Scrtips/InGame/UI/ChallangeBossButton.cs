using DI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallangeBossButton : DIMono
{
    public Button btn;
    ChallangeToBossStage enterBossStage = new ChallangeToBossStage();

    public override void Init()
    {
        btn.onClick.AddListener(
            () =>
            {
                EventBus.Publish(enterBossStage);
            });
    }
}
