using System;
using Entity;
using UnityEngine;

namespace Skills.Fireball
{
    public class FireballSkill : BaseSkill
    {
        [SerializeField] private GameObject fireballPrefab;
        [SerializeField] private int damage;

        private Cooldown skillCool;

        private void Awake()
        {
            skillCool = new Cooldown(cooldown);
        }
        
        public override void Activate(GameObject user, GameObject target)
        {
            
            if (skillCool.IsIn())
            {
                Debug.Log("Skill is on cooldown!");
                return;
            }
            skillCool.Start();
            base.Activate(user, target);
        }
        

        protected override void Execute(GameObject user, GameObject target)
        {
            GameObject fireballInstance = Instantiate(fireballPrefab, user.transform.position, Quaternion.identity);

            FireballController fireballController = fireballInstance.GetComponent<FireballController>();
            if (fireballController != null)
            {
                fireballController.Init(target, damage);
            }
            
        }
        

    }
}