using System.Manager;
using Skills.Iceage;
using UnityEngine;

/// <summary>
/// ToVetor3로 작동하며, target은 아무거나 넣어도 ㄱㅊ
/// </summary>
public class IceageSkill : BaseSkill
{
    [SerializeField] private GameObject iceagePrefab;
    [SerializeField] private int damage;

    private void Awake()
    {
        skillCool = new Cooldown(cooldown);
    }

    void Update() {
        if (skillCool.IsIn()) {
            UIManager.Instance.skill3Col.fillAmount = skillCool.timeLeft() / skillCool.time;
        } else {
            UIManager.Instance.skill3Col.fillAmount = 0;
        }
    }
        
    public override bool ActivateToVector3(GameObject user, Vector3 target)
    {
        return base.ActivateToVector3(user, target);
    }
        

    protected override void ExecuteToVector3(GameObject user, Vector3 target)
    {
        GameObject fireballInstance = Instantiate(iceagePrefab, user.transform.position, Quaternion.identity);

        IceageController iceageController = fireballInstance.GetComponent<IceageController>();
        if (iceageController != null)
        {
            iceageController.Init(target,damage);
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
