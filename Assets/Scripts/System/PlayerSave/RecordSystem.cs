using System.Collections.Generic;
using System.Linq;
using Entity.Player;
using UnityEngine;

namespace System.PlayerSave
{
    [System.Serializable]
    public class PlayerState : IEquatable<PlayerState>
    { 
        // 기본 위치 정보
        public Vector2 position;
        public Vector2 currentVelocity;
        
        // 시간 정보
        public float timestamp;
        public float deltaTime;
        
        
        // 입력 정보
        public HashSet<KeyCode> activeKeys;
        
        public PlayerState(Vector2 position, Vector2 velocity,
            float timestamp, float deltaTime, params KeyCode[] keys) {
            
            this.position = position;
            this.currentVelocity = velocity;
            this.timestamp = timestamp;
            this.deltaTime = deltaTime;
            this.activeKeys = new HashSet<KeyCode>(keys);
            
        }
        
        public PlayerState() {
            this.position = Vector2.zero;
            this.currentVelocity = Vector2.zero;
            this.activeKeys = new HashSet<KeyCode>();
        }
        
        public void AddKey(KeyCode key) => activeKeys.Add(key);
        public void RemoveKey(KeyCode key) => activeKeys.Remove(key);
        public bool HasKey(KeyCode key) => activeKeys.Contains(key);
        
        
        public override bool Equals(object obj) => Equals(obj as PlayerState);
        
        public bool Equals(PlayerState other) {
            if (other == null) return false;
            return timestamp == other.timestamp &&
                   position == other.position;
        }
        
        public override int GetHashCode() {
            return timestamp.GetHashCode() ^ 
                   position.GetHashCode();
        }
        
        public static PlayerState Lerp(PlayerState a, PlayerState b, float t) {
            PlayerState result = new PlayerState {
                position = Vector2.Lerp(a.position, b.position, t),
                currentVelocity = Vector2.Lerp(a.currentVelocity, b.currentVelocity, t),
                timestamp = Mathf.Lerp(a.timestamp, b.timestamp, t),
                deltaTime = Mathf.Lerp(a.deltaTime, b.deltaTime, t),
            };
            
            // 키 입력은 보간하지 않고 더 나중 상태 사용
            result.activeKeys = t < 0.5f ? 
                new HashSet<KeyCode>(a.activeKeys) : 
                new HashSet<KeyCode>(b.activeKeys);
                
            return result;
        }
        
        // 실제 이동 적용
        public void ApplyMovement(float playbackDeltaTime) {
            if (playbackDeltaTime <= 0) return;
            
            // deltaTime 비율에 따른 보정
            float timeRatio = playbackDeltaTime / deltaTime;
            position += currentVelocity * playbackDeltaTime * timeRatio;
        }

        public override string ToString() { return $"위치: {position.ToString()}\n가속도: {currentVelocity.ToString()}\n타임 스탬프: {timestamp.ToString()}"; }
    }

    public class RecordSystem : MonoBehaviour {

        public static RecordSystem Instance { get; private set; }

        // 기본값들
        private List<PlayerState> playerStates;
        public static PlayerState dummyState;

        // 플레이어 chase
        public Deque<PlayerState> before; // 전 프레임들을 통합하여(최대 n개 정도?) 멈춤 여부를 판단한다. 2의 제곱수로 만드는게 효율적 만든 deque가 그런 구조요...
        public bool onSkill; // 이동 자체를 스킬 중에 사용한다는 것은 말이 안됨. 스킬로 이동한다면 확인 필요. -> 이거 플레이어가 boolean으로 관리 해준ㄷ다 우효ㅗㅛㅛㅗㅗㅛ
        public int countOfBefore = 10;
        
        
        // other
        // 플레이어 직방 입력 시 넣으세용
        public PlayerController player { set; private get;  }

        // 초기화
        public void Awake() {
            playerStates = new List<PlayerState>();
            before = new Deque<PlayerState>();
            dummyState = new PlayerState {
                position = Vector2.zero,
                currentVelocity = Vector2.zero,
                timestamp = -1f,    // timestamp가 무조건 양수이므로 -1로 더미 표시
                deltaTime = 0f,
                activeKeys = new HashSet<KeyCode>()
            };
            Instance = this;
        }

        
        // 레코드 Add 값
        public void Record(params KeyCode[] keyCodes) {
            if (player == null) return;
            
            // 새로운 상태 생성
            PlayerState currentState = new PlayerState(
                player.transform.position,
                player.rigid.linearVelocity,
                Time.time,
                Time.deltaTime,
                keyCodes.ToArray()
            );
        
            Record(currentState);
        }
        public void Record(Vector2 position, Vector2 velocity, float time, float deltaTime, params KeyCode[] keys){
            Record(new PlayerState(position, velocity, time, deltaTime, keys));
        }

        public void Record(PlayerState currentState) {
            playerStates.Add(currentState);
            AddBefore(currentState);
        }

        
        
        private void AddBefore(PlayerState state) {
            if(before.Count >= countOfBefore) before.RemoveRear();
            
            before.AddFront(state);
        }
        
        
        
        // Util
        public List<PlayerState> GetRecordedStates() => new List<PlayerState>(playerStates);
        
        public void ClearRecords() {
            playerStates.Clear();
            before.Clear();
        }
        
        
        public PlayerState FindStateAtTime(float timestamp) => playerStates.FirstOrDefault(s => Mathf.Approximately(s.timestamp, timestamp)) ?? dummyState;
        public List<PlayerState> GetStatesBetween(float startTime, float endTime) => playerStates.Where(s => s.timestamp >= startTime && s.timestamp <= endTime).ToList();
        
        
        
        // Singleton 해제
        public void OnDestroy() { RecordSystem.Instance = null; }
    }
}