using System.Manager;
using Entity;
using Hunger_of_war.Util;
using UnityEngine;

namespace System.Weapon {
    public class WoodenSword : Weapon
    {
        Cooldown atkCool = new(1);
        Cooldown rightCool = new(4);
        DamageArea rightArea;
        DamageArea leftArea;

        public override string Id => "woodenSword";

        void Start() {
            rightArea = DamageArea.Init(transform, DamageAreaShape.FanShaped, 2.4f, 1.6f);
            leftArea = DamageArea.Init(transform, DamageAreaShape.FanShaped, 1.8f, 1.4f);
            rightArea.offset = new(0, -1);
            leftArea.offset = new(0, -1);
        }
        public override void OnAttatch()
        {
        }

        public override void OnUpdateTick()
        {
            rightArea.BecomeRed(rightCool.IsIn());
            leftArea.BecomeRed(atkCool.IsIn());
        }

        public override void OnMouseLeftDown()
        {
            leftArea.Show();
        }

        public override void OnMouseLeftkUp()
        {
            leftArea.Hide();
            if (atkCool.IsIn()) {
                return;
            }

            atkCool.Start();

            CamManager.main.Shake(1f);

            animator.SetTrigger("attack1");
            attatcher.animator.SetTrigger("attack1");

            foreach (BaseEntity target in leftArea.casted) {
                target.GetDamage(attatcher.baseDamage * 0.6f);
                target.KnockBack(transform.position, 1, 0.4f);
            }
        }

        public override void OnMouseRightDown()
        {
            rightArea.Show();
        }

        public override void OnMouseRightkUp()
        {
            rightArea.Hide();
            if (rightCool.IsIn()) {
                return;
            }

            rightCool.Start();

            CamManager.main.Shake(3f);

            animator.SetTrigger("attack_wide");
            attatcher.animator.SetTrigger("attack_wide");

            attatcher.stopMove = 0.3f;
            attatcher.Stop();

            foreach (BaseEntity target in rightArea.casted) {
                target.GetDamage(attatcher.baseDamage * 1.5f);
                target.KnockBack(transform.position, 12, 0.2f);
            }
        }
    }
}