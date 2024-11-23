
using Entity;
using UnityEngine;

namespace Skills.Missile
{
    /// <summary>
    /// 사용하기 위해서는 꼭! Projectile 컴포넌트를 추가해주세요!
    /// 사용하기 위해서는 꼭! 폭발 애니메이션 끝에 DestroyThis()를 호출해주세요!
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
            GameObject missileInstance = Instantiate(missilePrefab, user.transform.position, Quaternion.identity);

            MissileController missileController = missileInstance.GetComponent<MissileController>();
            if (missileController != null)
            {
                missileController.Init(target, damage);
            }
        }
        
    }
}