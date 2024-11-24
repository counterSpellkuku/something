

using UnityEngine;

namespace Entity.Monster {
    public class Goblin : Monster {
        protected override void MobUpdate()
        {

            if (state == MonsterState.Idle) {
                if (Dist(player.transform) <= 6) {
                    state = MonsterState.Chase;
                }
            }

            else if (state == MonsterState.Chase) {
                if (Dist(player.transform) <= 2f) {
                    Attack();
                } else {
                    
                    Chase(player.transform);
                }
            }
        }

        void Attack() {
            if (atkCool > 0) {
                return;
            }

            player.GetDamage(baseDamage, this);
            player.KnockBack(transform.position, 5, 0.1f);

            atkCool = 0.3f;

            animator.SetTrigger("attack");
            

            if (Random.Range(0, 100) < 50) {
                stopMove = 0.1f;

                Stop();
            }
        }
    }
}