using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireBallSkill : SkillBase
{
    public float moveSpeed;
    List<IXCollision> collisedObjs = new List<IXCollision>();

    public override void Init()
    {
        base.Init();
    }

    protected override void Update()
    {
        base.Update();
        transform.position += Vector3.right * moveSpeed * Time.deltaTime;
    }

    private void OnDisable()
    {
        collisedObjs.Clear();
    }

    /// <summary>
    /// 파이어볼의 경우 한 번 공격한 대상은 공격하지 않기 위해 공격한 적들을 리스트에 넣음.
    /// 그리고 그 리스트는 비활성화될 때 초기화시킴.
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator ActivateSkillCoroutine()
    {
        particle.Play();
        var x = this as IXCollision;

        while (true)
        {
            foreach (var enemy in mainObjs.EnemyUnits)
            {
                if (collisedObjs.Any(l => (object)l == enemy))
                    continue;

                if (x.IsCollide(enemy))
                {
                    enemy.TakeDamage(dmg);
                    collisedObjs.Add(enemy);
                }
            }
            yield return waitAttackDelay;
        }
    }
}
