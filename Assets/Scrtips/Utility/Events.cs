using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ø����̼� ����� ȣ��
/// </summary>
public class ApplicationQuitEvent { }
/// <summary>
/// ���� ���������� �̵�
/// </summary>
public class ChallangeToBossStage { }
/// <summary>
/// ���� �������� �����
/// </summary>
public class RestartCurrentStage { }
/// <summary>
/// ���� ���������� �̵�
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