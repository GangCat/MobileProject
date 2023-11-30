using DI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class PlayData 
{
    /// <summary>
    /// ���� ���������� ���� ������ ����
    /// </summary>
    public Stage currentStage;

    /// <summary>
    /// ��ų�� �ܿ� ��Ÿ���� ����
    /// </summary>
    public Dictionary<int, float> leftCooltimes =new Dictionary<int, float>();

    /// <summary>
    /// ���� ���������� ������ ������ ������������ �˷��ִ� �뵵
    /// </summary>
    public bool isBossStage;
}
/// <summary>
/// ���� ��ҿ��� �����ϱ� ���� Ŭ����
/// </summary>
public class MainObjs
{
    /// <summary>
    /// ���� �ʿ� �����ϴ� ������ ����
    /// </summary>
    public List<EnemyUnit> EnemyUnits { get; set; }

    /// <summary>
    /// ���� ������ ������ ����
    /// </summary>
    public HeroUnit HeroUnit { get; internal set; }
}



