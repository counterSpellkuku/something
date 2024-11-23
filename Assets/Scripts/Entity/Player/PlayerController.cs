using UnityEngine;

namespace Entity.Player
{
    public class PlayerController : BaseEntity {
        
        
        [Header("Movement Settings")]
        [SerializeField] private float acceleration = 50f;
        [SerializeField] private float deceleration = 50f;
        

        private Vector2 moveInput;
        private Vector2 currentVelocity;

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
            Move();
        }

        private void Move() {
            
            
            Vector2 targetVelocity = moveInput * speed;
            Debug.Log(targetVelocity);
            
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