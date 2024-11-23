using System.Collections;
using UnityEngine;

namespace Entity.Monster {
    public class Slime : Monster {
        float speedDef;
        protected override void MobStart()
        {
            speedDef = speed;
        }
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

            

            player.GetDamage(baseDamage * 0.8f, this);

            atkCool = 1f;
            stopMove = 1f;
            

            rigid.linearVelocity = Vector2.zero;

            
        }
    }
}