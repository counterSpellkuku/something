using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
    [SerializeField] protected string skillName;
    [SerializeField] protected float cooldown;

    public virtual void Activate(GameObject user, GameObject target)
    {
        Debug.Log($"{skillName} cast by {user.name}");
        Execute(user, target);
    }

    protected abstract void Execute(GameObject user, GameObject target);
}