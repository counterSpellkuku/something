using System;
using System.Reflection;
using UnityEngine;

namespace Entity
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class BaseEntity : MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField] public string name;
        [SerializeField] protected float currentHp, maxHp;
        [SerializeField] protected float speed, maxSpeed;
        
        [Header("Component")]
        [SerializeField] protected Rigidbody2D rigid;
        [SerializeField] protected BoxCollider2D collider;
        
        [Header("RigidBody Settings")]
        [SerializeField] private float acceleration = 50f;
        [SerializeField] private float deceleration = 50f;
        [SerializeField] protected Vector2 currentVelocity;


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
        
        protected void Move(Vector2 moveInput) {
            Vector2 targetVelocity = moveInput * speed;
            
            if (moveInput != Vector2.zero)
            {
                currentVelocity = Vector2.MoveTowards(
                    currentVelocity,
                    targetVelocity,
                    acceleration * Time.fixedDeltaTime
                );
            }
            else
            {
                currentVelocity = Vector2.MoveTowards(
                    currentVelocity,
                    Vector2.zero,
                    deceleration * Time.fixedDeltaTime
                );
            }

            currentVelocity = Vector2.ClampMagnitude(currentVelocity, maxSpeed);

            rigid.linearVelocity = currentVelocity;
        }


    }
}