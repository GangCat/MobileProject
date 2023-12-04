using DI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class EnemyUnitManager : DIMono
{
    [Inject]
    GameData gameData;

    [Inject]
    ObjectPoolManager objectPoolMng;

    [Inject]
    PlayData playdata;

    [Inject]
    MainObjs mainObjs;

    [Inject]
    UserData userData;

    List<EnemyUnit> EnemyUnits => mainObjs.EnemyUnits;

    [Inject("EnemyPrefabPath")]
    string EnemyUnitPath;
    [Inject("BossEnemyPrefabPath")]
    string BossEnemyUnitPath;

    public float spawnEnemyCnt;
    public float enemyOffset;
    public float enemystatPosOffset;
    public float bossSpawnOffset;

    bool isRestartStage;
    GameObject bossEnemyGo;
    
    public override void Init()
    {
        isRestartStage = false;
        if (playdata.isBossStage)
            PrepareBossStage();
        else
            PrepareStage();
    }

    void PrepareStage()
    {
        foreach(var mob in playdata.currentStage.Monsters)
        {
            objectPoolMng.PrepareObjects(mob.path);
        }

        SpawnEnemy();
        SpawnEliteEnemy();
    }

    /// <summary>
    /// ������ ��ȯ�� �� ȣ��
    /// �� ������Ʈ Ǯ�� �������� ����� ��.
    /// ���Ŀ� ������ ��
    /// </summary>
    void PrepareBossStage()
    {
        foreach (var mob in playdata.currentStage.Monsters)
        {
            objectPoolMng.PrepareObjects(mob.path);
        }

        SpawnBossEnemy();
    }

    /// <summary>
    /// �������� �⺻ ���� ����
    /// </summary>
    public void SpawnEnemy()
    {
        var curStage = playdata.currentStage;

        for (int i = 0 ;i < spawnEnemyCnt; ++i)
        {
            var mobData = curStage.Monsters[UnityEngine.Random.Range(0, curStage.Monsters.Count)];
            var mobPath = mobData.path;
            
            GameObject enemyGo = objectPoolMng.GetObject(EnemyUnitPath);
            enemyGo.transform.position = mainObjs.HeroUnit.transform.position + new Vector3((i * enemyOffset) + enemystatPosOffset, 0);

            var enemyUnit = enemyGo.GetComponent<EnemyUnit>();
            enemyUnit.SetData(mobData);
            EnemyUnits.Add(enemyUnit);

            // ���⼭ * 2�� ����, + 10�� �Ÿ� ������
        }
    }

    /// <summary>
    /// �������� ���� ���� ����
    /// </summary>
    public void SpawnEliteEnemy()
    {
        var curStage = playdata.currentStage;

        var mobData = gameData.bossMonsters.Find(l=>l.code==curStage.eliteMonsterCode);
        var mobPath = mobData.path;

        GameObject stageBossGo = objectPoolMng.GetObject(BossEnemyUnitPath);
        stageBossGo.transform.position = mainObjs.HeroUnit.transform.position + new Vector3(bossSpawnOffset, 0);

        var stageBossUnit = stageBossGo.GetComponent<EliteAndBossEnemyUnit>();
        stageBossUnit.SetData(mobData);
        EnemyUnits.Add(stageBossUnit);
    }

    /// <summary>
    /// �������� ���� ����
    /// </summary>
    public void SpawnBossEnemy()
    {
        var curStage = playdata.currentStage;

        var mobData = gameData.bossMonsters.Find(l => l.code == curStage.bossCode);
        var mobPath = mobData.path;

        GameObject stageBossGo = objectPoolMng.GetObject(BossEnemyUnitPath);
        stageBossGo.transform.position = mainObjs.HeroUnit.transform.position + new Vector3(bossSpawnOffset, 0);

        var stageBossUnit = stageBossGo.GetComponent<EliteAndBossEnemyUnit>();
        stageBossUnit.SetData(mobData);
        EnemyUnits.Add(stageBossUnit);
    }

    public void Update()
    {
        if (isRestartStage)
            return;

        if (mainObjs.EnemyUnits.Count < 1)
        {
            isRestartStage = true;
            if (playdata.isBossStage)
                EventBus.Publish(new ChangeToNextStage());
            else if (playdata.currentStage.type == SpaceType.Dungeon)
            {
                var dungeon = gameData.dungeons.Find(l => l.stageCode == playdata.currentStage.code);
                foreach(var r in dungeon.reward.listRewards)
                {
                    userData.IncrCurrency(r);
                }

                EventBus.Publish(new ReturnToLastNormalStage());
            }
            else
                EventBus.Publish(new RestartCurrentStage());
            return;
        }


        for(int i=mainObjs.EnemyUnits.Count - 1;i>= 0; --i)
        {
            var enemy = mainObjs.EnemyUnits[i];
            if (enemy.IsAlive())
                continue;

            mainObjs.EnemyUnits.Remove(enemy);
            ++playdata.currentKilledEnemyCount;
        }
    }
}
