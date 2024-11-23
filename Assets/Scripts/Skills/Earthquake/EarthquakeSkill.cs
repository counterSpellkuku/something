using System.Manager;
using UnityEngine;

namespace Skills.Earthquake
{
    /// <summary>
    /// ToVetor3로 작동하며, target은 아무거나 넣어도 ㄱㅊ
    /// </summary>
    public class EarthquakeSkill : BaseSkill
    {
        [SerializeField] private GameObject earthquakePrefab; // 지진 프리팹
        [SerializeField] private float damage = 10f; // 스킬 데미지

        void Update() {
            if (skillCool.IsIn()) {
                UIManager.Instance.skill1Col.fillAmount = skillCool.timeLeft() / skillCool.time;
            } else {
                UIManager.Instance.skill1Col.fillAmount = 0;
            }
        }
        
        
        public override bool ActivateToVector3(GameObject user, Vector3 target)
        {
            return base.ActivateToVector3(user, target);
        }

        protected override void ExecuteToVector3(GameObject user, Vector3 target)
        {
            Vector2 userPosition = user.transform.position;

            GameObject earthquakeInstance = Instantiate(earthquakePrefab, userPosition, Quaternion.identity);
            EarthquakeController earthquakeController = earthquakeInstance.GetComponent<EarthquakeController>();
            if (earthquakeController != null)
            {
                earthquakeController.Init(damage);
            }
        }
        
        protected override void ExecuteToObject(GameObject user, GameObject target)
        {
            throw new System.NotImplementedException();
        }
        
        public override bool ActivateToObject(GameObject user, GameObject target)
        {
            throw new System.NotImplementedException();
        }
        

        
    }
}