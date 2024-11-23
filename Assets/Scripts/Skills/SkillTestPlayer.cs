using Skills;
using Skills.Earthquake;
using Skills.Fireball;
using Skills.Missile;
using UnityEngine;

public class SkillTestPlayer : MonoBehaviour
{
    
    [SerializeField] private FireballSkill fireballSkill;
    [SerializeField] private MissileSkill missileSkill;
    [SerializeField] private EarthquakeSkill earthquakeSkill;
    
    [SerializeField] private GameObject target;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(earthquakeSkill == null)
            {
                Debug.LogError("skill is not set!");
                return;
            }
            
            earthquakeSkill.Activate(gameObject, gameObject);
        }
    }
}
