
using System.Manager;
using Entity;
using UnityEngine;

namespace Skills.Missile
{
    /// <summary>
    /// 사용하기 위해서는 꼭! Projectile 컴포넌트를 추가해주세요!
    /// 사용하기 위해서는 꼭! 폭발 애니메이션 끝에 DestroyThis()를 호출해주세요!
    /// ToObject로 작동하며, 무조건 target을 향해 발사합니다. 
    /// </summary>
    public class MissileSkill : BaseSkill
    {
        
        [SerializeField] private GameObject missilePrefab;
        [SerializeField] private int damage;
        
        
        private Cooldown skillCool;

        private void Awake()
        {
            skillCool = new Cooldown(cooldown);
        }

        void Update() {
            if (skillCool.IsIn()) {
                UIManager.Instance.skill4Col.fillAmount = skillCool.timeLeft() / skillCool.time;
            }
        }
        
        public override bool ActivateToObject(GameObject user, GameObject target)
        {
            return base.ActivateToObject(user, target);
        }
        
        protected override void ExecuteToObject(GameObject user, GameObject target)
        {
            GameObject missileInstance = Instantiate(missilePrefab, user.transform.position, Quaternion.identity);

            MissileController missileController = missileInstance.GetComponent<MissileController>();
            if (missileController != null)
            {
                missileController.Init(target, damage);
            }
        }

        protected override void ExecuteToVector3(GameObject user, Vector3 targetVec)
        {
            throw new System.NotImplementedException();
        }

        override public bool ActivateToVector3(GameObject user, Vector3 targetVec)
        {
            throw new System.NotImplementedException();
        }

    }   
}