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
