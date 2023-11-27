using DI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//유닛들의 각종 시각적 효과(이펙트, 데미지폰트, 체력 등)을 관리
public class UnitVisual : DIMono
{
    // 매개변수가 딱히 없으면 이렇게해도 됨
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
    /// 사망시 호출할 함수 등록
    /// </summary>
    /// <param name="_deathAction"></param>
    public void SetDeathAction(System.Action _deathAction)
    {
        deathAction = _deathAction;
    }

    /// <summary>
    /// 공격시 호출할 함수 등록
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
    /// 피격시 몸이 번쩍하는 효과
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
    /// 자신의 오프셋으로 자신의 위치 조정
    /// </summary>
    public void SetPosition()
    {
        transform.localPosition = offset;
    }


}
