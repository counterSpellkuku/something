using UnityEngine;

namespace Entity.Player
{
    public class PlayerController : BaseEntity {
        
        
        
        private Vector2 moveInput;

        private void Start() {
            rigid = GetComponent<Rigidbody2D>();
            rigid.gravityScale = 0f;
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        private void Update() {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
            moveInput = moveInput.normalized;
            
        }

        private void FixedUpdate() {
            Move(moveInput);
        }

        
        
       
        
        
        
        
        
        
        
        
        
        
        
    }
}