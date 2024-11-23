using System;
using System.Reflection;
using UnityEngine;

namespace Entity
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class BaseEntity : MonoBehaviour
    {
        [SerializeField] public string name;
        [SerializeField] protected float currentHp, maxHp;
        [SerializeField] protected float speed, maxSpeed;
        [SerializeField] protected Rigidbody2D rigid;
        [SerializeField] protected BoxCollider2D collider;


        public void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
            collider = GetComponent<BoxCollider2D>();
        }

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