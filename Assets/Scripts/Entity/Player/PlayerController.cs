using System.Collections.Generic;
using System.Linq;
using System.Manager;
using System.PlayerSave;
using UnityEngine;

namespace Entity.Player {
    public class PlayerController : BaseEntity {
        private Vector2 moveInput;
        public float inSkill, atkCool, preventInput, stopMove;
        protected override Color damageColor => Color.red;

        private HashSet<KeyCode> keys;
        
        private void Start() {
            rigid = GetComponent<Rigidbody2D>();
            rigid.gravityScale = 0f;
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
            RecordSystem.Instance.player = this;
            keys = new HashSet<KeyCode>();
            

        }

        private void Update() {

            if (preventInput <= 0) {
                moveInput.x = Input.GetAxisRaw("Horizontal");
                if(moveInput.x != 0) keys.Add(moveInput.x > 0 ? KeyCode.D : KeyCode.A);
                moveInput.y = Input.GetAxisRaw("Vertical");
                if(moveInput.y != 0) keys.Add(moveInput.y > 0 ? KeyCode.W : KeyCode.S);
                moveInput = moveInput.normalized;
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
            
        }

        private void FixedUpdate() {
             if (stopMove <= 0) {
                Move(moveInput);
                RecordSystem.Instance.Record(keys.ToArray());
                keys.Clear();
            }
        }

        public void OnDestroy() {
            // FileManager.SaveData("PlayerShadow", RecordSystem.Instance.GetRecordedStates().ToArray());
        }
        
        
        
    }
}