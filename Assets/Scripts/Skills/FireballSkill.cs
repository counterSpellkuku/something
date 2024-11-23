using System;
using UnityEngine;

namespace Skills
{
    public class FireballSkill : BaseSkill
    {
        [SerializeField] private GameObject fireballPrefab;

        private Cooldown skillCool;

        private void Awake()
        {
            skillCool = new Cooldown(cooldown);
        }
        

        protected override void Execute(GameObject user, GameObject target)
        {
            Debug.Log($"{skillName} cast by {user.name} on {target.name}");

            GameObject fireballInstance = Instantiate(fireballPrefab, user.transform.position, Quaternion.identity);

            FireballController fireballController = fireballInstance.GetComponent<FireballController>();
            if (fireballController != null)
            {
                fireballController.Init(target);
            }
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
    }
}