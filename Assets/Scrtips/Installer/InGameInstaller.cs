using DI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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

    GameObject backgroundObj;

    DIContainer container;

    [Inject]
    GameData gameData;
    [Inject]
    PlayData playData;

    private void Awake()
    {
        DIContainer.Inject(this);
        // ���� ���÷� �� �����̳� ����
        container = new DIContainer();
        // ���÷� ����
        DIContainer.AddContainer(container);
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


        var userData=DIContainer.GetObjT<UserData>();

        userData.skillCodePerLv.Clear();
        userData.equippedSkillCodes.Clear();
        foreach(var sk in initSkillCodes)
        {
            userData.skillCodePerLv[sk.code] = sk.level;
            if (sk.isEquipped)
            {
                userData.equippedSkillCodes.Add(sk.code);
            }
        }

        // �̸� ������Ʈ Ǯ���� ����.
        objectPoolMng.PrepareObjects("Assets/Prefab/Enemy.prefab");
        objectPoolMng.PrepareObjects("Assets/Prefab/BossEnemy.prefab");
        objectPoolMng.PrepareObjects("Assets/Prefab/P_DamageFont.prefab");
        container.Regist("Assets/Prefab/P_DamageFont.prefab", "DamageFontPath");



        LoadBackground();
    }

    GameObject bgPrefab;
    private void LoadBackground()
    {
        bgPrefab = Addressables.LoadAssetAsync<GameObject>(playData.currentStage.backgroundPath).WaitForCompletion();
        backgroundObj = Instantiate(bgPrefab);
    }

    private void OnDestroy()
    {
        Addressables.Release(bgPrefab);

        DIContainer.RemoveContainer(container);
    }
}
