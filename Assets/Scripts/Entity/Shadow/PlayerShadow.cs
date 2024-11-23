using System;
using System.Collections.Generic;
using System.Manager;
using System.PlayerSave;
using Mono.Cecil.Cil;
using Skills.Earthquake;
using Skills.Fireball;
using Skills.Missile;
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
        }
        

        public void FixedUpdate() {
            base.FixedUpdate();
            
            Vector2 dir = Vector2.zero;
            foreach (KeyCode code in currentState.activeKeys)
            {
                dir += GetDirection(code);
                CheckSkill(code);
            }
            
            
            
            // 스킬 체크 로직 추가
            Move(dir.normalized);
            if(states.Count > 1+idx)
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

        public void CheckSkill(KeyCode code) {
            Debug.Log(code);
            switch (code) {
                case KeyCode.Alpha1:
                    earth.Activate(this.gameObject, this.gameObject);
                    break;
                case KeyCode.Alpha2:
                    // fire.Activate(this.gameObject, );
                    break;
                case KeyCode.Alpha3:
                    ice.Activate(this.gameObject, this.gameObject);
                    break;
                case KeyCode.Alpha4:
                    missile.Activate(this.gameObject, GameManager.Instance.player.gameObject);
                    break;
            }
            
        }
        
        protected override void OnHurt(float damage, BaseEntity attacker, ref bool cancel) { cancel = true; }
        
    }
}