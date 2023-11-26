using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DI;
using TMPro;

public class HeroHpText : DIMono
{
    [Inject]
    MainObjs mainObjs;

    TMP_Text text;
    HeroUnit heroUnit;

    int prevHP;
    int prevMaxHP;

    public override void Init()
    {
        text = GetComponent<TMP_Text>();
        heroUnit = mainObjs.HeroUnit;
        UpdateText();
    }

    private void Update()
    {
        if(prevHP != (int)heroUnit.HP || prevMaxHP != (int)heroUnit.maxHP)
            UpdateText();
    }

    void UpdateText()
    {
        text.text = string.Format("{0:#0} / {1:#0}", heroUnit.HP, heroUnit.maxHP);
        prevHP = (int)heroUnit.HP;
        prevMaxHP = (int)heroUnit.maxHP;
    }
}
