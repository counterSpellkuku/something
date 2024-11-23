using System.Collections;
using UnityEngine;

namespace Entity.Monster {
    public class Slime : Monster {
        float time;
        protected override void MobUpdate()
        {
            if (state == MonsterState.Idle) {
                if (Dist(player.transform) <= 6) {
                    state = MonsterState.Chase;
                }
            }

            else if (state == MonsterState.Chase) {
                time += Time.deltaTime;

                if (time > 1f) {
                    time = 0;
                    stopMove = 0.5f;
                }
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

            float distance = Dist(player.transform);
            

            rigid.linearVelocity = moveDelta * distance * 2;

            yield return new WaitForSeconds(0.3f);

            Stop();

            if (Dist(player.transform) <= 1.3f) {
                player.GetDamage(baseDamage * 0.8f, this);

            }
        }
    }
}