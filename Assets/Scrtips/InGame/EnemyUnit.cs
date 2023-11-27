using DI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        base.Init();
    }

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

    public void TakeDamage(float _dmg)
    {
        curHP -= _dmg;
        if (curHP <= 0)
        {
            UnitAnim.PlayAni(AniKind.Dead);
            // Remove하면 배열에서 빠져서 플레이어가 인식하지 않음.
            MainObjs.EnemyUnits.Remove(this);
        }

        //데미지 폰트를 오브젝트 풀링으로 관리하며 데미지 출력
        GameObject damageFontGo = poolManager.GetObject(DamageFontPath);
        damageFontGo.transform.position = unitVisual.damageFontTf.position;
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
