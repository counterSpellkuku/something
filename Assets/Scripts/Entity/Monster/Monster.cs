using Entity.Player;
using UnityEngine;

namespace Entity.Monster {
    public enum MonsterState {
        Idle,
        Chase,
        Wait,
        Skill,
    }
    public class Monster : BaseEntity
    {
        [SerializeField]
        protected PlayerController player;
        public bool playerFound => player != null;

        public float inSkill, atkCool, stopMove;
        public int baseDamage;

        public MonsterState state;

        void Start()
        {
            player = FindFirstObjectByType<PlayerController>();

            MobStart();
        }

        // Update is called once per frame
        void Update()
        {
            if (inSkill > 0)
                inSkill -= Time.deltaTime;
            if (atkCool > 0)
                atkCool -= Time.deltaTime;
            if (stopMove > 0)
                stopMove -= Time.deltaTime;

            MobUpdate();
        }

        protected virtual void MobStart() {}
        protected virtual void MobUpdate() {}

        public void Chase(Transform target) {
            if (stopMove > 0) {
                return;
            }
            
            Vector2 moveDelta = Vector2.zero;

            if (target.position.x > transform.position.x) {
                moveDelta.x = 1;
            } else if (target.position.x < transform.position.x) {
                moveDelta.x = -1;
            }

            if (target.position.y > transform.position.y) {
                moveDelta.y = 1;
            } else if (target.position.y < transform.position.y) {
                moveDelta.y = -1;
            }


            Move(moveDelta);
        }

        protected float Dist(Transform target) {
            return Vector2.Distance(transform.position, target.position);
        }
    }

}