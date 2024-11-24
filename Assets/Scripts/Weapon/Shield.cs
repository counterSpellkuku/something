using System.Collections;
using System.Manager;
using Entity;
using Hunger_of_war.Util;
using UnityEngine;

namespace System.Weapon {
    public class Shield : Weapon
    {
        Cooldown atkCool = new(1);
        DamageArea leftArea;

        bool isShielding;
        float shieldTime;

        public override string Id => "shield";

        void Start() {
            leftArea = DamageArea.Init(transform, DamageAreaShape.Square, 1.5f, 1f);
        }
        public override void OnAttatch()
        {
        }

        public override void OnUpdateTick()
        {
            leftArea.BecomeRed(atkCool.IsIn());

            animator.SetBool("shielding", isShielding);

            if (isShielding) {
                attatcher.isMoving = true;
                attatcher.DirectionToMouse();

                attatcher.stopMove = 0.2f;

                shieldTime += Time.deltaTime;
            } else {
                shieldTime = 0;
            }
        }

        public override void OnMouseLeftDown()
        {
            if (isShielding) return;
            leftArea.Show();
        }

        public override void OnMouseLeftkUp()
        {
            leftArea.Hide();
            if (isShielding) return;

            if (atkCool.IsIn()) {
                return;
            }

            atkCool.Start();

            CamManager.main.Shake(1f);

            animator.SetTrigger("attack1");
            attatcher.animator.SetTrigger("attack1");

            foreach (BaseEntity target in leftArea.casted) {
                target.GetDamage(attatcher.baseDamage * 0.1f, attatcher);
                target.KnockBack(transform.position, 10, 0.4f);
            }
        }

        public override void OnHurt(float damage, BaseEntity attacker, ref bool cancel)
        {
            if (isShielding) {
                if (shieldTime <= 0.3f) {
                    cancel = true;

                    StartCoroutine(shielded(attacker, damage));
                }
            }
        }

        IEnumerator shielded(BaseEntity attacker, float damage) {
            attacker.KnockBack(attatcher.transform.position, 5, 0.2f);
            attacker.GetDamage(damage, attatcher);
            attatcher.KnockBack(attacker.transform.position, 2, 0.3f);

            CamManager.main.CloseUp(4f, 4 * attatcher.facing, 0.2f);

            CamManager.main.Shake(4, 0.3f);

            attatcher.stopMove = 0.5f;

            yield return new WaitForSeconds(0.4f);

            CamManager.main.CloseOut(0.2f);

            isShielding = false;
        }

        public override void OnMouseRightDown()
        {
            isShielding = true;

            attatcher.Stop();

            CamManager.main.CloseUp(5.5f, 0, 0.1f);
        }

        public override void OnMouseRightkUp()
        {
            isShielding = false;

            CamManager.main.CloseOut(0.1f);
        }
    }
}