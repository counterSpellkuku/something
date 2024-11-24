using System.Collections.Generic;
using Entity.Monster;
using UnityEngine;
using Util;

namespace System.Wave {
    
    [RequireComponent(typeof(BoxCollider2D))]
    public class WaveTrigger : MonoBehaviour {
        public BoxCollider2D boxCollider { private set;  get; }
        public SpriteRenderer renderer;
        public WaveData data;
        public bool isColliding { private set; get; }


        public float currentDelay;
        public float delay = 1.5f;

        public bool complete;

        public int maxMonster;
        public void Awake()
        {
            data = GetComponent<WaveData>();
            isColliding = false;
            boxCollider = GetComponent<BoxCollider2D>();
            
            if (boxCollider == null) {
                boxCollider = gameObject.AddComponent<BoxCollider2D>();
            }

            render = GetComponent<SpriteRenderer>();
            boxCollider.isTrigger = true;
            complete = false;

            if (this.gameObject.name == "First")
            {
                render.enabled = false;
                GetComponent<Collider2D>().isTrigger = true;
            }
            else {
                Debug.Log(this.gameObject);
                render.enabled = true;
                GetComponent<Collider2D>().isTrigger = false;
            }

            maxMonster = 0;
            foreach (KeyValuePair<GameObject, int> entry in data.monsters)
            {
                maxMonster += entry.Value;


            }
        }

        protected void Start() {
           
            // WaveSystem.Instance.colliders.Add(this);
            
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
                if (WaveSystem.Instance != null) {
                    WaveSystem.Instance.Event(this);
                }
            }

        
        }

        public void DelaySetup()
        {
            currentDelay = -UnityEngine.Random.Range(0.5f, 2.1f);
            
        }
        
        
    }

    
   
}