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
        if(prevHP != (int)heroUnit.curHP || prevMaxHP != (int)heroUnit.MaxHp)
            UpdateText();
    }

    void UpdateText()
    {
        text.text = string.Format("{0:#0} / {1:#0}", heroUnit.curHP, heroUnit.MaxHp);
        prevHP = (int)heroUnit.curHP;
        prevMaxHP = (int)heroUnit.MaxHp;
    }
}
