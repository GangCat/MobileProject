using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DI;
using UnityEngine.UI;

public class HeroHpFront : DIMono
{
    [Inject]
    MainObjs mainObjs;

    Image image;

    public override void Init()
    {
        image = GetComponent<Image>();
        image.fillAmount = 1f;
    }

    private void Update()
    {
        image.fillAmount = mainObjs.HeroUnit.HpRatio;
    }
}
