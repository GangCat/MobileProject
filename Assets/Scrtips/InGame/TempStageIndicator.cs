using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DI;
using TMPro;

public class TempStageIndicator : DIMono
{
    public TMP_Text text;

    [Inject]
    PlayData playData;

    private void Update()
    {
        text.text = playData.currentStage.name;
    }
}
