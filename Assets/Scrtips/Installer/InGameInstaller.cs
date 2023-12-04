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

    // 여기서 현재 씬에서 사용할 것들을 받아놓고 Regist해준다.
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
        // 먼저 로컬로 쓸 컨테이너 생성
        container = new DIContainer();
        // 로컬로 지정
        DIContainer.AddContainer(container);
        // 컨테이너에 로컬에서 필요한 내용 등록
        // 이때 좌측이 값, 우측이 키이며 키가 없어도 된다.
        // 하지만 자료형이 같으면 키가 필요하다.
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

        // 미리 오브젝트 풀링을 생성.
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
