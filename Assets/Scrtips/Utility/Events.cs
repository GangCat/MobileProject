using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 어플리케이션 종료시 호출
/// </summary>
public class ApplicationQuitEvent { }
/// <summary>
/// 보스 스테이지로 이동
/// </summary>
public class ChallangeToBossStage { }
/// <summary>
/// 현재 스테이지 재시작
/// </summary>
public class RestartCurrentStage { }
/// <summary>
/// 다음 스테이지로 이동
/// </summary>
public class ChangeToNextStage { }
public class ReturnToLastNormalStage { }
public class ErrorMessageEvent
{
    public ErrorMessageEvent(string code)
    {
        this.etcStrCode = code;
    }

    public string etcStrCode;
}

public class EnterToDungeon { public Dungeon dungeon; }