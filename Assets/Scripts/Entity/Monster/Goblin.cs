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
            if (atkCool > 0 || !playerFound) {
                return;
            }

            player.GetDamage(baseDamage * 0.8f, this);

            atkCool = 0.3f;
        }
    }
}