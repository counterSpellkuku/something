using System.Collections.Generic;
using System.Linq;
using System.Manager;
using System.PlayerSave;
using System.Weapon;
using UnityEngine;

namespace Entity.Player {
    public class PlayerController : BaseEntity {
        private Vector2 moveInput;
        public float inSkill, atkCool, preventInput, stopMove;
        protected override Color damageColor => Color.red;
        public int facing;
        public float baseDamage;
        public bool isMoving;
        public Weapon heldWeapon;

        private HashSet<KeyCode> keys;
        
        private void Start() {
            rigid = GetComponent<Rigidbody2D>();
            rigid.gravityScale = 0f;
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
            RecordSystem.Instance.player = this;
            keys = new HashSet<KeyCode>();

            facing = 1;
        }

        private void Update() {

            if (preventInput <= 0) {
                moveInput.x = Input.GetAxisRaw("Horizontal");
                if(moveInput.x != 0) keys.Add(moveInput.x > 0 ? KeyCode.D : KeyCode.A);
                moveInput.y = Input.GetAxisRaw("Vertical");
                if(moveInput.y != 0) keys.Add(moveInput.y > 0 ? KeyCode.W : KeyCode.S);

                isMoving = Mathf.Abs(moveInput.x) + Mathf.Abs(moveInput.y) != 0;

                moveInput = moveInput.normalized;
            } else {
                isMoving = false;
            }

            if (heldWeapon != null) {
                if (Input.GetMouseButtonDown(0)) {
                    heldWeapon.OnMouseLeftDown();
                }
                if (Input.GetMouseButtonUp(0)) {
                    heldWeapon.OnMouseLeftkUp();
                }
                if (Input.GetMouseButtonDown(1)) {
                    heldWeapon.OnMouseRightDown();
                }
                if (Input.GetMouseButtonUp(1)) {
                    heldWeapon.OnMouseRightkUp();
                }
            }

            // 플레이어 스킬마다 추가해야됨.
            
            


            if (inSkill > 0)
                inSkill -= Time.deltaTime;
            if (atkCool > 0)
                atkCool -= Time.deltaTime;
            if (preventInput > 0)
                preventInput -= Time.deltaTime;
            if (stopMove > 0)
                stopMove -= Time.deltaTime;
            
            Animate();
        }

        private void Animate() {
            animator.SetBool("isMoving", isMoving);
            shadowAnim.SetBool("isMoving", isMoving);
            if (moveInput.y > 0) {
                animator.SetInteger("faceY", 1);
                shadowAnim.SetInteger("faceY", 1);
            } else if (moveInput.y < 0) {
                animator.SetInteger("faceY", -1);
                shadowAnim.SetInteger("faceY", -1);
            } else {
                animator.SetInteger("faceY", 0);
                shadowAnim.SetInteger("faceY", 0);
            }

            if (moveInput.x > 0) {
                animator.SetBool("toRight", true);
                shadowAnim.SetBool("toRight", true);
                facing = 1;
            } else if (moveInput.x < 0) {
                animator.SetBool("toRight", true);
                shadowAnim.SetBool("toRight", true);
                facing = -1;
            } else {
                animator.SetBool("toRight", false);
                shadowAnim.SetBool("toRight", false);
            }

            if (facing == 1) {
                render.flipX = false;
            } else if (facing == -1) {
                render.flipX = true;
            }
        }

        protected override void OnHurt(float damage, BaseEntity attacker, ref bool cancel)
        {
            CamManager.main.Shake(0.2f);
        }

        private new void FixedUpdate() {
            base.FixedUpdate();
             if (stopMove <= 0) {
                Move(moveInput);
                RecordSystem.Instance.Record(keys.ToArray());
                keys.Clear();
            }
        }

        public void OnDestroy()
        {
        }

    }
}