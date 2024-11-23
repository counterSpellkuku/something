using System;
using System.Collections.Generic;
using System.Manager;
using System.PlayerSave;
using Mono.Cecil.Cil;
using UnityEngine;

namespace Entity.Shadow
{
    public class PlayerShadow : BaseEntity
    {
        
        private PlayerState currentState;
        private List<PlayerState> states;
        private int idx;
        public void Awake() {
            states = FileManager.GetShadow();
            
        }

        public void Start()
        {
            currentState = states[0];
            idx = 0;
        }

        public void Update() {
            
        }

        public void FixedUpdate() {
            Vector2 dir = Vector2.zero;
            foreach (KeyCode code in currentState.activeKeys) { dir += GetDirection(code); }
            
            // 스킬 체크 로직 추가
            Move(dir.normalized);
            if(states.Count < idx)
                currentState = states[++idx];

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
        
        protected override void OnHurt(float damage, BaseEntity attacker, ref bool cancel) { cancel = true; }
        
    }
}