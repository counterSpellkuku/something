using System;
using System.Manager;
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

        private void Awake()
        {
            skillCool = new Cooldown(cooldown);
        }

        void Update() {
            if (UIManager.Instance != null) {
                if (skillCool.IsIn()) {
                    UIManager.Instance.skill2Col.fillAmount = skillCool.timeLeft() / skillCool.time;
                } else {
                    UIManager.Instance.skill2Col.fillAmount = 0;
                }
            }
        }
        
        public override bool ActivateToVector3(GameObject user, Vector3 target)
        {
            return base.ActivateToVector3(user, target);
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
        
        public override bool ActivateToObject(GameObject user, GameObject target)
        {
            throw new NotImplementedException();
        }
    }
}