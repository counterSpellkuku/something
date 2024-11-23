using System.Manager;
using System.PlayerSave;
using Entity;
using Entity.Monster;
using Hunger_of_war.Util;
using UnityEngine;

namespace System.Weapon {
    public class Wand : Weapon
    {
        Cooldown atkCool = new(1.5f);
        Cooldown rightCool = new(5);
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

            // pj.transform.rotation = leftArea.rot.rotation;
            
            Vector3 dir = (RecordSystem.Instance.GetMousePosition() - this.transform.position).normalized;
            

            attatcher.DirectionToMouse();
            pj.rb.AddForce(dir * 8, ForceMode2D.Impulse);
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

            attatcher.stopMove = 0.5f;
            attatcher.Stop();

            Projectile pj = Instantiate(wandBullet, transform.position, Quaternion.identity);
            pj.LifeTime = 10;
            pj.transform.localScale = pj.transform.localScale * 2;

            // pj.transform.rotation = leftArea.rot.rotation;
            
            Vector3 dir = (RecordSystem.Instance.GetMousePosition() - this.transform.position).normalized;
            

            attatcher.DirectionToMouse();
            pj.rb.AddForce(dir * 15, ForceMode2D.Impulse);
            pj.OnHit = OnHitSkBullet;
        }

        void OnHitSkBullet(Transform target, Projectile pj) {
            Monster entity = target.GetComponent<Monster>();

            if (entity != null) {
                entity.GetDamage(attatcher.baseDamage * 0.7f, attatcher);
                entity.KnockBack(target.position, 6, 0.3f);
            }
        }
    }
}