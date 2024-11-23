using System.Collections;
using UnityEngine;

namespace Entity.Monster {
    public class Slime : Monster {
        protected override void MobUpdate()
        {
            if (state == MonsterState.Idle) {
                if (Dist(player.transform) <= 6) {
                    state = MonsterState.Chase;
                }
            }

            else if (state == MonsterState.Chase) {
                if (Dist(player.transform) <= 3f) {
                    StartCoroutine(Attack());
                } else {
                    Chase(player.transform);
                }
            }
        }

        IEnumerator Attack() {
            if (atkCool > 0) {
                yield break;
            }

            Chase(player.transform);

            atkCool = 1f;
            stopMove = 1f;

            animator.SetTrigger("attack");

            float distance = Dist(player.transform);
            

            rigid.linearVelocity = moveDelta * distance * 2;

            yield return new WaitForSeconds(0.3f);

            Stop();

            if (Dist(player.transform) <= 1.5f) {
                player.GetDamage(baseDamage, this);
                player.KnockBack(transform.position, 8, 0.2f);

            }
        }
    }
}