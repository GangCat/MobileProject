using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DI;
using System.IO;
using UnityEngine.AddressableAssets;
using System;
using System.Linq;

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


    private void OnEnable()
    {
        EventBus.Subscribe<SceneChangeEvent>(OnSceneChangeHandler);
        
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<SceneChangeEvent>(OnSceneChangeHandler);
    }


    private void OnSceneChangeHandler(SceneChangeEvent obj)
    {
        OnDestroy();
        Init();
    }
    
    /// <summary>
    /// �ش� �ε����� ��ġ�ϴ� ��ų �����ܿ� ��������Ʈ�� �������ֱ� ���� ��ų������ Ŭ������ ã�� �Լ�.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public SkillIcon GetSkillIcon(int index)
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

        skillCodeToSkillOffset.Clear();
        skillCodeToGameObject.Clear();

        for (idx=0;idx<userData.equippedSkillCodes.Count;idx++)
        {
            var skCode = userData.equippedSkillCodes[idx];
            var skill = gameData.GetSkill(skCode);

            // �������� ��ų ����
            var skObj = Addressables.InstantiateAsync(skill.fxPrerfabPath).WaitForCompletion();
            if(skObj.TryGetComponent<SkillBase>(out var skillBase))
            {
                skillBase.Init();
                skillBase.SetData(skill.damage, skill.coolTime);
            }
            
            skObj.SetActive(false);
            skillCodeToSkillOffset[skCode] = skObj.transform.position;
            skillCodeToGameObject[skCode] = skObj;
            LeftSkillCooltime[skCode] = skill.coolTime;
            GetSkillIcon(idx).SetData(skill);
        }
    }

    private void Update()
    {
        // ��� �������� ��ų ��ȸ
        foreach (var skCode in userData.equippedSkillCodes)
        {
            // �������̸� ��Ÿ���� �Ǿ��� ��
            if (LeftSkillCooltime[skCode] <= 0 && mainObjs.HeroUnit.isAttack)
            {
                skillCodeToGameObject[skCode].transform.position = mainObjs.HeroUnit.transform.position + skillCodeToSkillOffset[skCode];
                skillCodeToGameObject[skCode].SetActive(true);
                LeftSkillCooltime[skCode] = gameData.GetSkill(skCode).coolTime;

                // ���Ŀ� ��ų ���� �ٸ� ��ų ��Ÿ�� 0�ΰ͵� �� 0.5��? ���� �þ���� ���ֱ�.
                // �� ���� ��� ��ų�� ������� ���ϰ� �ϱ� ����.
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
