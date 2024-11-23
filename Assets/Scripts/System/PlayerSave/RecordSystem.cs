using System.Collections.Generic;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace System.PlayerSave
{
    [System.Serializable]
    public class PlayerState : IEquatable<PlayerState>{
        public Vector2 position;
        public float timestamp;
        public KeyCode[] keyCodes;
        
        public override bool Equals(object obj)
        {
            var other = obj as PlayerState;
            if( other == null ) return false;

            return Equals (other);
        }
        
        
        public bool Equals(PlayerState other) {
            if (other == null) return false;
            return other.timestamp == this.timestamp;
        }
    }
    
    public class RecordSystem {
        // 기본값들
        private List<PlayerState> playerStates;
        public static PlayerState dummyState;

        // 플레이어 chase
        public PlayerState[] before; // 전 프레임들을 통합하여(최대 n개 정도?) 멈춤 여부를 판단한다.
        public bool onSkill; // 이동 자체를 스킬 중에 사용한다는 것은 말이 안됨. 스킬로 이동한다면 확인 필요.
        
        
        
        
        
        
        
        
        
        public RecordSystem() {
            dummyState = new PlayerState();
            
            
        }

        public void Awake() {
            playerStates = new List<PlayerState>();
            
        }


        public void Record() {
            
        }
        
        
        
    }
}