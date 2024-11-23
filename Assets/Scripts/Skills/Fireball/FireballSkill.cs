using System;
using Entity;
using UnityEngine;

namespace Skills.Fireball
{
    /// <summary>
    /// ToVector3로 작동하며, 무조건 target을 향해 발사합니다. 
    /// </summary>
    public class FireballSkill : BaseSkill
    {
        [SerializeField] private GameObject fireballPrefab;
        [SerializeField] private int damage;

        private Cooldown skillCool;

        private void Awake()
        {
            skillCool = new Cooldown(cooldown);
        }
        
        public override void ActivateToVector3(GameObject user, Vector3 target)
        {
            
            if (skillCool.IsIn())
            {
                Debug.Log("Skill is on cooldown!");
                return;
            }
            skillCool.Start();
            base.ActivateToVector3(user, target);
        }
        

        protected override void ExecuteToVector3(GameObject user, Vector3 target)
        {
            GameObject fireballInstance = Instantiate(fireballPrefab, user.transform.position, Quaternion.identity);

            FireballController fireballController = fireballInstance.GetComponent<FireballController>();
            if (fireballController != null)
            {
                fireballController.Init(target, damage);
            }
            
        }

        protected override void ExecuteToObject(GameObject user, GameObject target)
        {
            throw new NotImplementedException();
        }
        
        public override void ActivateToObject(GameObject user, GameObject target)
        {
            throw new NotImplementedException();
        }
    }
}