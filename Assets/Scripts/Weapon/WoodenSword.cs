using Hunger_of_war.Util;
using UnityEngine;

namespace System.Weapon {
    public class WoodenSword : Weapon
    {
        Cooldown atkCool = new(1);
        DamageArea rightArea;
        void Start() {
            rightArea = DamageArea.Init(attatcher.transform, DamageAreaShape.Square, 1, 1);
        }
        public override void OnAttatch()
        {
        }

        public override void OnMouseLeftDown()
        {
            if (atkCool.IsIn()) {
                rightArea.Show();
            }
        }

        public override void OnMouseLeftkUp()
        {
            rightArea.Hide();
            if (atkCool.IsIn()) {
                return;
            }

            atkCool.Start();
        }

        public override void OnMouseRightDown()
        {
            
        }

        public override void OnMouseRightkUp()
        {
            
        }
    }
}