using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayData 
{
    public Stage currentStage;

    public Dictionary<int, float> leftCooltimes =new Dictionary<int, float>();

}
/// <summary>
/// 여러 장소에서 접근하기 위한 클래스
/// </summary>
public class MainObjs
{
    public List<EnemyUnit> EnemyUnits { get; set; }
    public HeroUnit HeroUnit { get; internal set; }
}