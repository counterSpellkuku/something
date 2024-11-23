using Skills.Iceage;
using UnityEngine;

public class IceageSkill : BaseSkill
{
    [SerializeField] private GameObject iceagePrefab;
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
        GameObject fireballInstance = Instantiate(iceagePrefab, user.transform.position, Quaternion.identity);

        IceageController iceageController = fireballInstance.GetComponent<IceageController>();
        if (iceageController != null)
        {
            iceageController.Init(target,damage);
        }
            
    }

}
