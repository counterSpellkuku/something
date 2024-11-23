using System.Manager;
using Entity;
using Entity.Monster;
using Hunger_of_war.Util;
using UnityEngine;

namespace System.Weapon {
    public class Wand : Weapon
    {
        Cooldown atkCool = new(1.5f);
        Cooldown rightCool = new(4);
        DamageArea rightArea;
        DamageArea leftArea;
        [SerializeField]
        Projectile wandBullet;

        public override string Id => "wand";

        void Start() {
            rightArea = DamageArea.Init(transform, DamageAreaShape.Square, 3f, 10f);
            leftArea = DamageArea.Init(transform, DamageAreaShape.Square, 1f, 10f);
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

            animator.SetTrigger("attack1");
            attatcher.animator.SetTrigger("attack1");

            Projectile pj = Instantiate(wandBullet, transform.position, Quaternion.identity);
            pj.LifeTime = 10;

            pj.transform.rotation = leftArea.rot.rotation;
            Debug.Log(pj.transform.forward);

            attatcher.DirectionToMouse();
            pj.rb.linearVelocity = pj.transform.forward * 2;
            pj.OnHit = OnHitDefBullet;
        }

        void OnHitDefBullet(Transform target, Projectile pj) {
            Monster entity = target.GetComponent<Monster>();

            if (entity != null) {
                entity.GetDamage(attatcher.baseDamage * 0.4f, attatcher);
                entity.KnockBack(target.position, 4, 0.3f);
            }

            Destroy(pj.gameObject);
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

            animator.SetTrigger("attack1");
            attatcher.animator.SetTrigger("attack1");

            attatcher.stopMove = 0.3f;
            attatcher.Stop();

            foreach (BaseEntity target in rightArea.casted) {
                target.GetDamage(attatcher.baseDamage * 1.5f);
                target.KnockBack(transform.position, 12, 0.2f);
            }
        }
    }
}