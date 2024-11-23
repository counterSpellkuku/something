using System.Collections.Generic;
using UnityEngine;
using Util;

namespace System.Wave {
    
    [RequireComponent(typeof(BoxCollider2D))]
    public class WaveTrigger : MonoBehaviour {
        public BoxCollider2D boxCollider { private set;  get; }
        [SerializeField] public WaveData data;
        public bool isColliding { private set; get; }


        public float currentDelay;
        public float delay = 1.5f;
        protected void Start() {
            isColliding = false;
            boxCollider = GetComponent<BoxCollider2D>();
            
            if (boxCollider == null) {
                boxCollider = gameObject.AddComponent<BoxCollider2D>();
            }
            
            WaveSystem.Instance.colliders.Add(this);
            boxCollider.isTrigger = true;
        }
        
        void OnTriggerEnter2D(Collider2D other)
        {
            isColliding = true;
            HandleCollision(other, "start");
        }

        void OnTriggerStay2D(Collider2D other)
        {
            HandleCollision(other, "ing");
        }

        void OnTriggerExit2D(Collider2D other)
        {
            isColliding = false;
            HandleCollision(other, "exit");
        }

        private void HandleCollision(Collider2D collision, string state)
        {
            // Debug.Log($"충돌 {state}: {collision.gameObject.name}");
            if (state == "ing" && collision.gameObject.layer == LayerMask.NameToLayer("player")) {
                WaveSystem.Instance.Event(this);
            }

        
        }

        public void DelaySetup()
        {
            currentDelay = -UnityEngine.Random.Range(0.5f, 2.1f);
            
        }
        
        
    }

    
    [CreateAssetMenu(fileName = "New Wave", menuName = "Compy/Wave Data")]
    [Serializable] public class WaveData : ScriptableObject {
        public string Name;
        public SerializableDictionary<GameObject, int> monsters;  
    }
}