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
        [SerializeField] public string Name;
        [SerializeField] protected float currentHp, maxHp;
        [SerializeField] protected float speed, maxSpeed;
        
        [Header("Component")]
        public Rigidbody2D rigid { get; protected set; }
        public BoxCollider2D col { get; protected set; }
        public Animator animator { get; protected set; }
        public SpriteRenderer render { get; protected set; }

        [Header("RigidBody Settings")] 
        [SerializeField] public float acceleration = 50f;
        [SerializeField] public float deceleration = 50f;
        [SerializeField] protected Vector2 currentVelocity;

        protected virtual Color damageColor => Color.white;

        private bool onknockBack;
        private float knockBackDuration;
        
        public void Awake() {
            rigid = GetComponent<Rigidbody2D>();
            col = GetComponent<BoxCollider2D>();
            animator = GetComponent<Animator>();
            render = GetComponent<SpriteRenderer>();
            onknockBack = false;
            knockBackDuration = 0;
        }
        
        protected virtual void FixedUpdate() {
            // 넉백 타이머 처리
            if (onknockBack) {
                knockBackDuration -= Time.fixedDeltaTime;
                if (knockBackDuration <= 0) {
                    onknockBack = false;
                    rigid.linearVelocity = Vector2.zero; // 넉백 종료 시 속도 초기화
                }
            }
        }
        

        public virtual bool GetDamage(float damage, BaseEntity attacker = null) {
            // 특수 상황인 경우 override
            bool cancel = false;
            OnHurt(damage, attacker, ref cancel);

            if (cancel) return false;

            DamageIndicator.Show(transform.position + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.2f, 0.2f) + 0.5f), damage, damageColor);
            
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
            if (onknockBack) {
                return;
            }
            
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

        public void Stop() {
            currentVelocity = Vector2.zero;
            rigid.linearVelocity = currentVelocity;
        }
        
        public void KnockBack(Vector3 position, float force, float duration) {
            onknockBack = true;
            knockBackDuration = duration;
            Vector3 direction = (transform.position - position).normalized;            
            rigid.linearVelocity = Vector2.zero;
            rigid.AddForce(direction * force, ForceMode2D.Impulse);
        }


    }
}