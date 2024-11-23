using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
    [SerializeField] protected string skillName;
    [SerializeField] protected float cooldown;

    public virtual void ActivateToObject(GameObject user, GameObject target)
    {
        Debug.Log($"{skillName} cast by {user.name}");
        ExecuteToObject(user, target);
    }
    
    public virtual void ActivateToVector3(GameObject user, Vector3 targetVec)
    {
        Debug.Log($"{skillName} cast by {user.name}");
        ExecuteToVector3(user, targetVec);
    }

    protected abstract void ExecuteToObject(GameObject user, GameObject target);
    
    protected abstract void ExecuteToVector3 (GameObject user, Vector3 targetVec);
}