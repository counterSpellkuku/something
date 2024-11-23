using UnityEngine;

namespace System.Weapon {
    public class WoodenSword : Weapon
    {
        Cooldown atkCool = new(1);
        public override void OnAttatch()
        {
        }

        public override void OnMouseLeftDown()
        {
            if (atkCool.IsIn()) {
                return;
            }

            atkCool.Start();
        }

        public override void OnMouseLeftkUp()
        {
            
        }

        public override void OnMouseRightDown()
        {
            
        }

        public override void OnMouseRightkUp()
        {
            
        }
    }
}