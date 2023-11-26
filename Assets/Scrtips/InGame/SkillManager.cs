using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DI;
using System.IO;
using UnityEngine.AddressableAssets;

public class SkillManager : DIMono
{
    [Inject]
    UserData userData;

    [Inject]
    PlayData playData;

    [Inject]
    GameData gameData;

    [Inject]
    MainObjs mainObjs;


    public Transform skillBtnParentTf;
    public Dictionary<int, float> LeftSkillCooltime => playData.leftCooltimes;
    public Dictionary<int, GameObject> skillCodeToGameObject = new Dictionary<int, GameObject>();
    public Dictionary<int, Vector3> skillCodeToSkillOffset = new Dictionary<int, Vector3>();

    public SkillIcon GetSkillBtn(int index)
    {
        while ( index >= skillBtnParentTf.childCount)
        {
            var proto = skillBtnParentTf.GetChild(0);

            Instantiate(proto, skillBtnParentTf);
        }

        return skillBtnParentTf.GetChild(index).GetComponent<SkillIcon>();
    }

    public override void Init()
    {
        int idx = 0;
        
        for(idx=0;idx<userData.equippedSkillCodes.Count;idx++)
        {
            var skCode = userData.equippedSkillCodes[idx];
            var skill = gameData.GetSkill(skCode);

            // 장착중인 스킬 생성
            var skObj = Addressables.InstantiateAsync(skill.fxPrerfabPath).WaitForCompletion();
            skObj.SetActive(false);
            skillCodeToSkillOffset[skCode] = skObj.transform.position;
            skillCodeToGameObject[skCode] = skObj;
            LeftSkillCooltime[skCode] = skill.coolTime;
            GetSkillBtn(idx).SetData(skill);
        }
    }

    private void Update()
    {
        foreach (var skCode in userData.equippedSkillCodes)
        {
            if (LeftSkillCooltime[skCode] <= 0)
            {
                skillCodeToGameObject[skCode].transform.position = mainObjs.HeroUnit.transform.position + skillCodeToSkillOffset[skCode];
                skillCodeToGameObject[skCode].SetActive(true);
                LeftSkillCooltime[skCode] = gameData.GetSkill(skCode).coolTime;
                continue;
            }
            LeftSkillCooltime[skCode] -= Time.deltaTime;

        }
    }

    private void OnDestroy()
    {
        foreach (var skillObj in skillCodeToGameObject.Values)
            Addressables.Release(skillObj);
    }



}
