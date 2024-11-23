using Skills;
using UnityEngine;

public class SkillTestPlayer : MonoBehaviour
{
    
    [SerializeField] private FireballSkill fireballSkill;
    [SerializeField] private GameObject target;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(fireballSkill == null)
            {
                fireballSkill = new FireballSkill();
            }
            fireballSkill.Activate(gameObject, target);
        }
    }
}
