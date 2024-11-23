using UnityEngine;

namespace Entity.Player
{
    public class PlayerController : BaseEntity {
        
        
        
        private Vector2 moveInput;
        public float inSkill, atkCool, preventInput;

        private void Start() {
            rigid = GetComponent<Rigidbody2D>();
            rigid.gravityScale = 0f;
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        private void Update() {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
            moveInput = moveInput.normalized;

            if (inSkill > 0)
                inSkill -= Time.deltaTime;
            if (atkCool > 0)
                atkCool -= Time.deltaTime;
            if (preventInput > 0)
                preventInput -= Time.deltaTime;
            
        }

        private void FixedUpdate() {
            if (preventInput <= 0)
                Move(moveInput);
        }

        
        
       
        
        
        
        
        
        
        
        
        
        
        
    }
}