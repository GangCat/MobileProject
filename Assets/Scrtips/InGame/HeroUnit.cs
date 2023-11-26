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
        HP -= Time.deltaTime;

        // ���� ���̻� ���� ���
        if (MainObjs.EnemyUnits.Count < 1)
        {
            HeroWalk();
            return;
        }
        // ���� ����� �ָ��ִ� ���
        if (Vector3.Distance(transform.position, MainObjs.EnemyUnits[0].transform.position) > 0.5f)
        {
            HeroWalk();
            return;
        }

        unitAnim.PlayAni(AniKind.Attack);
    }


    private void HeroWalk()
    {
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
