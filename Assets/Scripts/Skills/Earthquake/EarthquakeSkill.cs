using UnityEngine;

namespace Skills.Earthquake
{
    public class EarthquakeSkill : BaseSkill
    {
        [SerializeField] private GameObject earthquakePrefab; // 지진 프리팹
        [SerializeField] private float damage = 10f; // 스킬 데미지

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
            Vector2 userPosition = user.transform.position;

            GameObject earthquakeInstance = Instantiate(earthquakePrefab, userPosition, Quaternion.identity);
            EarthquakeController earthquakeController = earthquakeInstance.GetComponent<EarthquakeController>();
            if (earthquakeController != null)
            {
                earthquakeController.Init(damage);
            }
        }

        
    }
}