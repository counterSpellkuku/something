using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Manager;
using System.PlayerSave;
using System.Wave;
using System.Weapon;
using Skills.Earthquake;
using Skills.Fireball;
using Skills.Missile;
using UnityEngine;

namespace Entity.Player {
    
    [RequireComponent(typeof(EarthquakeSkill))]
    [RequireComponent(typeof(FireballSkill))]
    [RequireComponent(typeof(IceageSkill))]
    [RequireComponent(typeof(MissileSkill))]
    public class PlayerController : BaseEntity {
        private Vector2 moveInput;
        public float inSkill, atkCool, preventInput;
        protected override Color damageColor => Color.red;
        public int facing;
        public float baseDamage;
        public bool isMoving;
        public Weapon heldWeapon;
        int faceY;
        bool toRight;

        public EarthquakeSkill earth;
        public FireballSkill fire;
        public IceageSkill ice;
        public MissileSkill missile;

        [SerializeField] public DeadCanva canva;
        private HashSet<KeyCode> keys;

        Cooldown dashCool = new(1);
        
        private void Start() {
            rigid = GetComponent<Rigidbody2D>();
            rigid.gravityScale = 0f;
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
            RecordSystem.Instance.player = this;
            keys = new HashSet<KeyCode>();

            facing = 1;
            
            
            earth = GetComponent<EarthquakeSkill>();
            fire = GetComponent<FireballSkill>();
            ice = GetComponent<IceageSkill>();
            missile = GetComponent<MissileSkill>();
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
                    keys.Add(KeyCode.Mouse0);
                }
                if (Input.GetMouseButtonDown(1)) {
                    heldWeapon.OnMouseRightDown();
                }
                if (Input.GetMouseButtonUp(1)) {
                    heldWeapon.OnMouseRightkUp();
                    keys.Add(KeyCode.Mouse1);
                }
            }

            if (UIManager.Instance != null) {
                if (heldWeapon == null) {
                    UIManager.Instance.weaponBack.sprite = UIManager.Instance.weaponImg.sprite = null;
                    UIManager.Instance.weaponBack.gameObject.SetActive(false);
                }

                UIManager.Instance.hpRate.value = currentHp / maxHp;
                UIManager.Instance.hpTextBack.text = UIManager.Instance.hpText.text = ((int)currentHp).ToString() + "/" + ((int)maxHp).ToString();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift)) {
                if (!dashCool.IsIn()) {
                    dashCool.Start();
                    StartCoroutine(Dash());

                    SoundManager.Instance.Play("dash");
                }
            }

            
            // 플레이어 스킬마다 추가해야됨.

            
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                if(earth.ActivateToVector3(this.gameObject, Vector3.one))
                    keys.Add(KeyCode.Alpha1);
            } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
                if (fire.ActivateToVector3(this.gameObject, RecordSystem.Instance.GetMousePosition()))
                    keys.Add(KeyCode.Alpha2);
            } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
                if (ice.ActivateToVector3(this.gameObject, Vector3.one))
                    keys.Add(KeyCode.Alpha3);
            } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
                if (missile.ActivateToObject(this.gameObject, GetNearTarget().gameObject))
                    keys.Add(KeyCode.Alpha4);
            }
            

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

        IEnumerator Dash() {
            float sp = speed;

            speed *= 100;

            CamManager.main.CloseUp(5f, 0, 0f);

            yield return new WaitForSeconds(0.5f);

            CamManager.main.CloseOut(0.1f);

            speed = sp;
        }


        public Transform GetNearTarget() {
            Vector3 pos = RecordSystem.Instance.GetMousePosition();
            return Physics2D.OverlapCircle(pos, 5f, 1 << LayerMask.NameToLayer("monster"))?.transform;
        }

        private void Animate() {
            animator.SetBool("isMoving", isMoving);
            shadowAnim.SetBool("isMoving", isMoving);

            if (isMoving) {
                ApplyDirection(moveInput);
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

                if (Input.GetKeyDown(KeyCode.Q)) {
                    Destroy(heldWeapon.gameObject);
                    heldWeapon = null;
                }
            }
        }

        public void DirectionToMouse() {
            //DirectionTo()
        }

        public void DirectionTo(Vector3 pos) {
            ApplyDirection(pos - transform.position);
        }

        public void ApplyDirection(Vector2 direction) {
            if (direction.y > 0) {
                faceY = 1;
            } else if (direction.y < 0) {
                faceY = -1;
            } else {
                animator.SetInteger("faceY", 0);
                shadowAnim.SetInteger("faceY", 0);
            }

            if (direction.y != 0) {
                animator.SetInteger("faceY", faceY);
                shadowAnim.SetInteger("faceY", faceY);
            }

            if (direction.x > 0) {
                toRight = true;
                facing = 1;
            } else if (direction.x < 0) {
                toRight = true;
                facing = -1;
            } else {
                animator.SetBool("toRight", false);
                shadowAnim.SetBool("toRight", false);
            }

            if (direction.x != 0) {
                animator.SetBool("toRight", toRight);
                shadowAnim.SetBool("toRight", toRight);
            }
        }

        protected override void OnHurt(float damage, BaseEntity attacker, ref bool cancel)
        {
            if (heldWeapon != null) {
                heldWeapon.OnHurt(damage, attacker, ref cancel);
            }

            if (!cancel) {
                CamManager.main.Shake(0.2f);
                SoundManager.Instance.Play("hit");
            }
        }

        private new void FixedUpdate() {
            base.FixedUpdate();
            if (stopMove <= 0) {
                Move(moveInput);
                RecordSystem.Instance.Record(0, keys.ToArray());
                keys.Clear();
            }
        }

        public override void Dead() { canva.ShowGameOver(); }
        public void OnDestroy()
        {
        }

    }
}