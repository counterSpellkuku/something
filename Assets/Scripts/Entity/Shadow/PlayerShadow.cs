using System;
using System.Collections.Generic;
using System.Manager;
using System.PlayerSave;
using System.Weapon;
using Mono.Cecil.Cil;
using Skills.Earthquake;
using Skills.Fireball;
using Skills.Missile;
using Unity.VisualScripting;
using UnityEngine;

namespace Entity.Shadow
{
    [RequireComponent(typeof(EarthquakeSkill))]
    [RequireComponent(typeof(FireballSkill))]
    [RequireComponent(typeof(IceageSkill))]
    [RequireComponent(typeof(MissileSkill))]
    public class PlayerShadow : BaseEntity
    {
        
        private PlayerState currentState;
        private List<PlayerState> states;
        private int idx;


        public EarthquakeSkill earth;
        public FireballSkill fire;
        public IceageSkill ice;
        public MissileSkill missile;
        bool isMoving, toRight;
        int facing, faceY;
        Weapon heldWeapon;
        
        
        public new void Awake() {
            base.Awake();
            states = FileManager.GetShadow();
        }

        public void Start()
        {
            currentState = states[0];
            idx = 0;

            earth = GetComponent<EarthquakeSkill>();
            fire = GetComponent<FireballSkill>();
            ice = GetComponent<IceageSkill>();
            missile = GetComponent<MissileSkill>();
            animator = transform.AddComponent<Animator>();
            
            animator.runtimeAnimatorController = GameManager.Instance.player.animator.runtimeAnimatorController;
        }
        

        public void FixedUpdate() {
            base.FixedUpdate();
            
            Vector2 dir = Vector2.zero;
            foreach (KeyCode code in currentState.activeKeys)
            {
                dir += GetDirection(code);
                CheckSkill(code);
            }
            
            Animate(dir.normalized);
            
            // 스킬 체크 로직 추가
            Move(dir.normalized);
            if(states.Count > 1+idx)
                currentState = states[++idx];

        }

        private void Animate(Vector2 direction) {
            animator.SetBool("isMoving", isMoving);

            if (isMoving) {
                ApplyDirection(direction);
            }

            if (facing == 1) {
                render.flipX = false;
            } else if (facing == -1) {
                render.flipX = true;
            }

            if (heldWeapon != null) {
                heldWeapon.animator.SetBool("isMoving", isMoving);
                
                heldWeapon.animator.SetInteger("faceY", animator.GetInteger("faceY"));

                heldWeapon.animator.SetBool("toRight", animator.GetBool("toRight"));

                heldWeapon.render.flipX = render.flipX;
            }
        }

        public void ApplyDirection(Vector2 direction) {
            if (direction.y > 0) {
                faceY = 1;
            } else if (direction.y < 0) {
                faceY = -1;
            } else {
                animator.SetInteger("faceY", 0);
            }

            if (direction.y != 0) {
                animator.SetInteger("faceY", faceY);
            }

            if (direction.x > 0) {
                toRight = true;
                facing = 1;
            } else if (direction.x < 0) {
                toRight = true;
                facing = -1;
            } else {
                animator.SetBool("toRight", false);
            }

            if (direction.x != 0) {
                animator.SetBool("toRight", toRight);
            }
        }
        
        public Vector2 GetDirection(KeyCode code) {
            switch (code) {
                case KeyCode.A:
                    return Vector2.left;
                case KeyCode.D:
                    return Vector2.right;
                case KeyCode.W:
                    return Vector2.up;
                case KeyCode.S:
                    return Vector2.down;
            }
            return Vector2.zero;
        }
        
        

        public void CheckSkill(KeyCode code) {
            Debug.Log(code);
            Weapon weapon;
            switch (code) {
                case KeyCode.Alpha1:
                    earth.ActivateToVector3(this.gameObject, Vector3.one);
                    break;
                case KeyCode.Alpha2:
                    fire.ActivateToVector3(this.gameObject, currentState.mousePosition);
                    // fire.Activate(this.gameObject, );
                    break;
                case KeyCode.Alpha3:
                    ice.ActivateToVector3(this.gameObject, Vector3.one);
                    break;
                case KeyCode.Alpha4:
                    missile.ActivateToObject(this.gameObject, GameManager.Instance.player.gameObject);
                    break;
                
                case KeyCode.Mouse0 :
                    weapon = WeaponManager.FindById(currentState.weaponId.ToString());
                    if (weapon == null) return;
                    weapon.OnMouseLeftkUp();
                    break;
                case KeyCode.Mouse1:
                    weapon = WeaponManager.FindById(currentState.weaponId.ToString());
                    if (weapon == null) return;
                    weapon.OnMouseRightkUp();
                    break;
            }
            
        }
        
        protected override void OnHurt(float damage, BaseEntity attacker, ref bool cancel) { cancel = true; }
        
    }
}