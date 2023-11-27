using DI;
using System.Collections;
using System.Collections.Generic;
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

    List<EnemyUnit> EnemyUnits => mainObjs.EnemyUnits;

    [Inject("EnemyPrefabPath")]
    string EnemyUnitPath;
    [Inject("BossEnemyPrefabPath")]
    string BossEnemyUnitPath;

    public float spawnEnemyCnt;
    public float enemyOffset;
    public float enemystatPosOffset;
    public float bossSpawnOffset;
    
    public override void Init()
    {
        PrepareStage();
    }

    void PrepareStage()
    {
        foreach(var mob in playdata.currentStage.Monsters)
        {
            objectPoolMng.PrepareObjects(mob.path);
        }

        SpawnEnemy();
        SpawnStageBossEnemy();
    }

    void PrepareBossStage()
    {
        SpawnBossEnemy();
    }

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

            // 여기서 * 2가 간격, + 10이 거리 오프셋
        }
    }

    public void SpawnStageBossEnemy()
    {
        var curStage = playdata.currentStage;

        var mobData = gameData.bossMonsters.Find(l=>l.code==curStage.bossCode);
        var mobPath = mobData.path;

        GameObject stageBossGo = objectPoolMng.GetObject(BossEnemyUnitPath);
        stageBossGo.transform.position = mainObjs.HeroUnit.transform.position + new Vector3(bossSpawnOffset, 0);

        var stageBossUnit = stageBossGo.GetComponent<StageBossEnemyUnit>();
        stageBossUnit.SetData(mobData);
        EnemyUnits.Add(stageBossUnit);
    }

    public void SpawnBossEnemy()
    {

    }

    public void Update()
    {
        for(int i=mainObjs.EnemyUnits.Count - 1;i>= 0; --i)
        {
            var enemy = mainObjs.EnemyUnits[i];
            if (enemy.IsAlive())
                continue;

            mainObjs.EnemyUnits.Remove(enemy);
        }
    }

}
