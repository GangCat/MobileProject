using DI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using static UnitAnim;

public class EnemyUnit : DIMono, IXCollision
{
    [Inject]
    protected ObjectPoolManager poolManager;

    [Inject]
    protected MainObjs MainObjs;

    [Inject("DamageFontPath")]
    public string DamageFontPath;

    public float maxHP;
    public float curHP;
    public GaugeBar healthBar;
    public float width;

    protected UnitAnim _unitAni;
    protected UnitAnim UnitAnim
    {
        get
        {
            if (_unitAni == null)
            {
                _unitAni = new UnitAnim();
            }
            return _unitAni;
        }
    }

    public float CenterX => transform.position.x;

    public float Width => width;

    public UnitVisual unitVisual;

    protected Monster mobData;
    public override void Init()
    {
    }

    /// <summary>
    /// 각종 필요한 정보들을 초기화
    /// </summary>
    /// <param name="mobData"></param>
    protected internal void SetData(Monster mobData)
    {
        CheckAndInject();
        this.mobData = mobData;

        unitVisual = poolManager.GetObject(mobData.path).GetComponent<UnitVisual>();
        unitVisual.SetDeathAction(ReturnToPool);
        healthBar.transform.localPosition = unitVisual.hpBarTf.localPosition;
        UnitAnim.SetAnimator(unitVisual.GetComponent<Animator>());

        unitVisual.transform.SetParent(this.transform);
        unitVisual.SetPosition();
        //unitVisual.transform.localPosition = Vector3.zero;

        maxHP = mobData.health;
        curHP = maxHP;

    }

    /// <summary>
    /// 생존을 확인해 죽었을 경우 EnemyManager가 오브젝트풀에 돌려주도록 구현
    /// </summary>
    /// <returns></returns>
    public bool IsAlive()
    {
        return curHP > 0;
    }

    /// <summary>
    /// 데미지 피격시 이펙트
    /// </summary>
    /// <param name="_dmg"></param>
    public void TakeDamage(float _dmg)
    {
        curHP -= _dmg;
        if (curHP <= 0)
        {
            UnitAnim.PlayAni(AniKind.Dead);
        }

        //데미지 폰트를 오브젝트 풀링으로 관리하며 데미지 출력
        GameObject damageFontGo = poolManager.GetObject(DamageFontPath);
        Vector3 rndPos = unitVisual.damageFontTf.position + new Vector3(UnityEngine.Random.Range(-0.2f, 0.2f), 0, 0);
        damageFontGo.transform.position = rndPos;
        damageFontGo.GetComponent<DamageFont>().Show(_dmg, Color.white);
        healthBar.UpdateLength(curHP / maxHP);

        StartCoroutine(unitVisual.DamageFx());
    }

    public void ReturnToPool()
    {
        poolManager.ReturnObj(unitVisual.gameObject);
        unitVisual = null;
        poolManager.ReturnObj(this.gameObject);
    }

}
