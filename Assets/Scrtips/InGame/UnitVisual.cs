using DI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���ֵ��� ���� �ð��� ȿ��(����Ʈ, ��������Ʈ, ü�� ��)�� ����
public class UnitVisual : DIMono
{
    // �Ű������� ���� ������ �̷����ص� ��
    System.Action deathAction;
    System.Action attackAction;

    public Transform damageFontTf,hpBarTf;
    public Vector3 offset;

    [Inject("Default")]
    Material defaultMat;

    [Inject("OneColor")]
    Material oneColorMat;

    SpriteRenderer _spRenderer;
    WaitForSeconds flashSec = new WaitForSeconds(0.1f);

    public SpriteRenderer SpriteRenderer
    {
        get
        {
            if (_spRenderer == null)
            {
                _spRenderer = GetComponent<SpriteRenderer>();
            }
            return _spRenderer;

        }
    }

    /// <summary>
    /// ����� ȣ���� �Լ� ���
    /// </summary>
    /// <param name="_deathAction"></param>
    public void SetDeathAction(System.Action _deathAction)
    {
        deathAction = _deathAction;
    }

    /// <summary>
    /// ���ݽ� ȣ���� �Լ� ���
    /// </summary>
    /// <param name="_attackAction"></param>
    public void SetAttackAction(System.Action _attackAction)
    {
        attackAction = _attackAction;
    }

    public void Death()
    {
        deathAction?.Invoke();
    }

    public void AttackHero()
    {
        attackAction?.Invoke();
    }

    /// <summary>
    /// �ǰݽ� ���� ��½�ϴ� ȿ��
    /// </summary>
    /// <returns></returns>
    public IEnumerator DamageFx()
    {
        SpriteRenderer.sharedMaterial = oneColorMat;
        SpriteRenderer.color = Color.red;
        yield return flashSec;
        SpriteRenderer.color = Color.white;
        SpriteRenderer.sharedMaterial = defaultMat;
    }

    /// <summary>
    /// �ڽ��� ���������� �ڽ��� ��ġ ����
    /// </summary>
    public void SetPosition()
    {
        transform.localPosition = offset;
    }


}
