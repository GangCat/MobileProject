using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayData 
{
    public Stage currentStage;

    public Dictionary<int, float> leftCooltimes =new Dictionary<int, float>();

}
/// <summary>
/// ���� ��ҿ��� �����ϱ� ���� Ŭ����
/// </summary>
public class MainObjs
{
    public List<EnemyUnit> EnemyUnits { get; set; }
    public HeroUnit HeroUnit { get; internal set; }
}