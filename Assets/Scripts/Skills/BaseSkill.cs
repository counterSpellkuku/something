using System;
using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
    [SerializeField] protected string skillName;
    [SerializeField] protected float cooldown;
    
    private Cooldown skillCool;

    private void Awake()
    {
        skillCool = new Cooldown(cooldown);
    }


    public virtual bool ActivateToObject(GameObject user, GameObject target)
    {
        if (skillCool.IsIn())
        {
            Debug.Log("Skill is on cooldown!");
            return false;
        }
        
        skillCool.Start();
        Debug.Log($"{skillName} cast by {user.name}");
        ExecuteToObject(user, target);

        return true;
    }
    
    public virtual bool ActivateToVector3(GameObject user, Vector3 targetVec)
    {
        if (skillCool.IsIn())
        {
            Debug.Log("Skill is on cooldown!");
            return false;
        }
        
        skillCool.Start();
        Debug.Log($"{skillName} cast by {user.name}");
        ExecuteToVector3(user, targetVec);
        
        return true;
    }

    protected abstract void ExecuteToObject(GameObject user, GameObject target);
    
    protected abstract void ExecuteToVector3 (GameObject user, Vector3 targetVec);
}