using DI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameInstaller : MonoBehaviour
{
    [Serializable]
    public class SkillCodeAndLv
    {
        public int code, level;
        public bool isEquipped;
    }

    // ���⼭ ���� ������ ����� �͵��� �޾Ƴ��� Regist���ش�.
    public Camera mainCam;
    
    public HeroUnit heroUnit;
    public ObjectPoolManager objectPoolMng;

    public Material oneColorMat,spriteDefaultMat;

    public List<SkillCodeAndLv> initSkillCodes;

    private void Awake()
    {
        // ���� ���÷� �� �����̳� ����
        var container = new DIContainer(); 
        // ���÷� ����
        DIContainer.Local = container;
        // �����̳ʿ� ���ÿ��� �ʿ��� ���� ���
        // �̶� ������ ��, ������ Ű�̸� Ű�� ��� �ȴ�.
        // ������ �ڷ����� ������ Ű�� �ʿ��ϴ�.
        container.Regist(mainCam);
        container.Regist(heroUnit);
        container.Regist(objectPoolMng);
        container.Regist("Assets/Prefab/Enemy.prefab", "EnemyPrefabPath");
        container.Regist("Assets/Prefab/BossEnemy.prefab", "BossEnemyPrefabPath");
        container.Regist(oneColorMat, "OneColor");
        container.Regist(spriteDefaultMat, "Default");
        container.Regist(new MainObjs()
        {
            HeroUnit= heroUnit,
            EnemyUnits= new List<EnemyUnit>()
        });

        var userdata=DIContainer.GetObj<UserData>();

        userdata.skillCodePerLv.Clear();
        userdata.equippedSkillCodes.Clear();
        foreach(var sk in initSkillCodes)
        {
            userdata.skillCodePerLv[sk.code] = sk.level;
            if (sk.isEquipped)
            {
                userdata.equippedSkillCodes.Add(sk.code);
            }
        }



        objectPoolMng.PrepareObjects("Assets/Prefab/Enemy.prefab");
        objectPoolMng.PrepareObjects("Assets/Prefab/BossEnemy.prefab");
        objectPoolMng.PrepareObjects("Assets/Prefab/P_DamageFont.prefab");
        container.Regist("Assets/Prefab/P_DamageFont.prefab", "DamageFontPath");
    }
}
