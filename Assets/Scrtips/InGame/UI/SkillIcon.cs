using DI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class SkillIcon : DIMono
{
    [Inject]
    PlayData playData;

    Image iconImage;
    Image cooltimeMaskImage;
    Skill skill;
    float oriCooltime;
    internal void SetData(Skill skill)
    {
        iconImage = GetComponent<Image>();
        cooltimeMaskImage = transform.GetChild(0).GetComponent<Image>();
        this.skill = skill;
        iconImage.sprite = skill.IconImage;
        oriCooltime = skill.coolTime;
    }

    private void Update()
    {
        cooltimeMaskImage.fillAmount = playData.leftCooltimes[skill.code] / oriCooltime;
    }

    private void OnDestroy()
    {
        // 여기서 릴리즈하니까 잘 안됨.
        // 그래서 다른곳으로 옮김.
        //Addressables.Release(skill.IconImage);
        //iconImage.sprite = null;
       // skill.ReleaseIcon();
        //Destroy(iconImage.gameObject);
    }
}
