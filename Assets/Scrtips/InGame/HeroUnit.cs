using DI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnitAnim;

public class HeroUnit : DIMono
{
    public float damage;
    public float HP;
    public float maxHP;
    public bool isAttack;

    [Inject]
    MainObjs MainObjs;

    UnitAnim unitAnim;

    public Animator animator;

    public float HpRatio => HP / maxHP;


    public override void Init()
    {
        unitAnim = new UnitAnim(animator);
        unitAnim.PlayAni(AniKind.Walk);
        HP = maxHP;
    }

    public void Update()
    {
        HP -= Time.deltaTime * 100;

        // 적이 더이상 없는 경우
        if (MainObjs.EnemyUnits.Count < 1)
        {
            HeroWalk();
            return;
        }
        // 적이 충분히 멀리있는 경우
        if (Vector3.Distance(transform.position, MainObjs.EnemyUnits[0].transform.position) > 0.5f)
        {
            HeroWalk();
            return;
        }

        isAttack = true;
        unitAnim.PlayAni(AniKind.Attack);
    }


    private void HeroWalk()
    {
        isAttack = false;
        unitAnim.PlayAni(AniKind.Walk);
        if(!unitAnim.IsAttack)
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
    }

    public void AttackEnemy()
    {
        if (MainObjs.EnemyUnits.Count == 0)
            return;

        MainObjs.EnemyUnits[0].TakeDamage(damage);
    }


    public float moveSpeed;
}
