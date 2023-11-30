using DI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class PlayData 
{
    /// <summary>
    /// 현재 스테이지의 각종 정보를 저장
    /// </summary>
    public Stage currentStage;

    /// <summary>
    /// 스킬의 잔여 쿨타임을 관리
    /// </summary>
    public Dictionary<int, float> leftCooltimes =new Dictionary<int, float>();

    /// <summary>
    /// 현재 스테이지가 보스가 출현할 스테이지인지 알려주는 용도
    /// </summary>
    public bool isBossStage;
}
/// <summary>
/// 여러 장소에서 접근하기 위한 클래스
/// </summary>
public class MainObjs
{
    /// <summary>
    /// 현재 맵에 존재하는 적들을 저장
    /// </summary>
    public List<EnemyUnit> EnemyUnits { get; set; }

    /// <summary>
    /// 영웅 유닛의 정보를 저장
    /// </summary>
    public HeroUnit HeroUnit { get; internal set; }
}



