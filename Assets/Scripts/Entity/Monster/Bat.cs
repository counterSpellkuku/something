using UnityEngine;

namespace Entity.Monster {
public class Bat : Monster
{
        protected override void MobUpdate()
        {

            if (state == MonsterState.Idle) {
                if (Dist(player.transform) <= 6) {
                    state = MonsterState.Chase;
                }
            }


            else if (state == MonsterState.Chase) {
                if (Dist(player.transform) <= 1.3f) {
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

            GetHeal(2);

            atkCool = 0.5f;
            stopMove = 0.2f;
        }
    }
}