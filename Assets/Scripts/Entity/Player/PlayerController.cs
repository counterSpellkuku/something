using System.Collections.Generic;
using System.Linq;
using System.Manager;
using System.PlayerSave;
using NUnit.Framework;
using UnityEngine;

namespace Entity.Player {
    public class PlayerController : BaseEntity {
        private Vector2 moveInput;
        public float inSkill, atkCool, preventInput;

        private HashSet<KeyCode> keys;
        
        private void Start() {
            rigid = GetComponent<Rigidbody2D>();
            rigid.gravityScale = 0f;
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
            RecordSystem.Instance.player = this;
            keys = new HashSet<KeyCode>();
            List<PlayerState> list = FileManager.GetShadow();
            foreach (PlayerState state in list)
            {
                Debug.Log(state);
            }
        }

        private void Update() {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            if(moveInput.x != 0) keys.Add(moveInput.x > 0 ? KeyCode.D : KeyCode.A);
            moveInput.y = Input.GetAxisRaw("Vertical");
            if(moveInput.y != 0) keys.Add(moveInput.y > 0 ? KeyCode.W : KeyCode.S);
            // 플레이어 스킬마다 추가해야됨.
            moveInput = moveInput.normalized;
            
            if (inSkill > 0)
                inSkill -= Time.deltaTime;
            if (atkCool > 0)
                atkCool -= Time.deltaTime;
            if (preventInput > 0)
                preventInput -= Time.deltaTime;
            
            if(Input.GetKeyDown(KeyCode.Space)) FileManager.SaveData("PlayerShadow", RecordSystem.dummyState, RecordSystem.dummyState);
        }

        private void FixedUpdate() {
            if (preventInput <= 0) {
                Move(moveInput);
                RecordSystem.Instance.Record(keys.ToArray());
                keys.Clear();
            }
        }
        
        
        
    }
}