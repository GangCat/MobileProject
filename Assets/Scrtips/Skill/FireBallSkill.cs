using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireBallSkill : SkillBase
{
    public float moveSpeed;
    List<IXCollision> collisedObjs;

    public override void Init()
    {
        base.Init();
        collisedObjs = new List<IXCollision>();
    }

    protected override void Update()
    {
        base.Update();
        transform.position += Vector3.right * moveSpeed * Time.deltaTime;
    }

    protected override void OnEnable()
    {
        collisedObjs.Clear();
        base.OnEnable();
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
            yield return null;
        }


    }
}
