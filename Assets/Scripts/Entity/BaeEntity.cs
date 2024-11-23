using System.Reflection;
using UnityEngine;

namespace Entity
{
    public class BaseEntity : MonoBehaviour
    {
        [SerializeField] public string name;
        [SerializeField] protected float currentHp, maxHp;
        [SerializeField] protected float speed;

        
        
        
        public virtual bool GetDamage(float damage, BaseEntity attacker) {
            // 특수 상황인 경우 override
            bool cancel = false;
            OnHurt(damage, attacker, ref cancel);

            if (cancel) return false;
            
            currentHp -= damage;
            if (currentHp <= 0f)
                Dead();

            return true;
        }

        protected virtual void OnHurt(float damage, BaseEntity attacker, ref bool cancel) {
            
        }

        public virtual void GetHeal(float heal) {
            if (currentHp + heal > maxHp) currentHp = maxHp;
            else currentHp += heal;
        }
        
        public virtual void Dead() {
            
        }


    }
}