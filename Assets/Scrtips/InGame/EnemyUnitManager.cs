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

    public float spawnEnemyCnt;
    public float enemyOffset;
    public float enemystatPosOffset;
    
    public override void Init()
    {
        // ���� ���������� �غ�
        PrepareStage();
    }

    void PrepareStage()
    {
        foreach(var mob in playdata.currentStage.Monsters)
        {
            objectPoolMng.PrepareObjects(mob.path);
        }


        SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        var curStage = playdata.currentStage;

        for (int i = 0 ;i < spawnEnemyCnt; ++i)
        {
            var mobData = curStage.Monsters[UnityEngine.Random.Range(0, curStage.Monsters.Count)];
            var mobPath = mobData.path;
            
            GameObject enemyGo = objectPoolMng.GetObject(EnemyUnitPath);
            var enemyUnit = enemyGo.GetComponent<EnemyUnit>();
            enemyUnit.SetData(mobData);
            EnemyUnits.Add(enemyUnit);

            // ���⼭ * 2�� ����, + 10�� �Ÿ� ������
            enemyGo.transform.position = mainObjs.HeroUnit.transform.position + new Vector3((i * enemyOffset) + enemystatPosOffset, 0);
        }
    }

   
}
