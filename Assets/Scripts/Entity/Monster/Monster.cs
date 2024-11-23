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
        protected Vector2 moveDelta;

        public int facing = 1;
        int faceDef;

        void Start()
        {
            player = FindFirstObjectByType<PlayerController>();

            faceDef = facing;

            facing = 1 * faceDef;

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

            if (facing == 1) {
                render.flipX = false;
            } else if (facing == -1) {
                render.flipX = true;
            }

            MobUpdate();
        }

        protected virtual void MobStart() {}
        protected virtual void MobUpdate() {}

        public void Chase(Transform target) {
            moveDelta = Vector2.zero;
            if (stopMove > 0) {
                return;
            }

            if (target.position.x > transform.position.x) {
                moveDelta.x = 1;
            } else if (target.position.x < transform.position.x) {
                moveDelta.x = -1;
            }

            facing = (int)moveDelta.x * faceDef;

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