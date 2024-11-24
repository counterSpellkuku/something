using System.Collections.Generic;
using System.Manager;
using Entity;
using Entity.Monster;
using UnityEngine;
using UnityEngine.Tilemaps;
using Util;

namespace System.Wave
{
    
    public class WaveSystem : MonoBehaviour {
        public static WaveSystem Instance;
        [SerializeField] public List<WaveTrigger> colliders;
        [SerializeField] public SerializableDictionary<string, List<Vector2>> validPosition;

        int maxMonster;
        
        public Tilemap groundTilemap;
        public LayerMask layer;
        
        public void Awake() { Instance = this; }

        public void Start() {
            foreach (WaveTrigger wave in colliders)
                ValidCheck(wave);
        }

        private void ValidCheck(WaveTrigger wave) {
            BoxCollider2D boxCollider = wave.boxCollider;
            Bounds bounds = boxCollider.bounds;
            Vector2 bottomLeft = bounds.min;
            Vector2 topRight = bounds.max;
            // Debug.Log(bottomLeft+" "+ topRight);
            float spawnAreaInterval = UnityEngine.Random.Range(1f, 2.5f);
            List<Vector2> w = new List<Vector2>();
            
            for(float x = bottomLeft.x; x <= topRight.x; x += spawnAreaInterval) {
                for(float y = bottomLeft.y; y <= topRight.y; y += spawnAreaInterval) {
                    Vector3Int cellPosition = groundTilemap.WorldToCell(new Vector3(x, y, 0));
                    if(groundTilemap.HasTile(cellPosition)) {
                        if ("TileMap_115" != groundTilemap.GetTile(cellPosition).name) continue;
                        Vector2 tileWorldPos = groundTilemap.GetCellCenterWorld(cellPosition);
                        Vector2 spawnPoint = tileWorldPos + Vector2.up * 0.5f; // 오프셋 조절 가능
                        w.Add(spawnPoint);
                        
                    // 현재 위치에서 아래로 타일 찾기
                    // while(cellPosition.y >= groundTilemap.cellBounds.yMin) {
                    //     Debug.Log("hello");
                    //     if(groundTilemap.HasTile(cellPosition)) {
                    //         if ("TileMap_115" != groundTilemap.GetTile(cellPosition).name) continue;
                    //         Vector2 tileWorldPos = groundTilemap.GetCellCenterWorld(cellPosition);
                    //         Vector2 spawnPoint = tileWorldPos + Vector2.up * 0.5f; // 오프셋 조절 가능
                    //         w.Add(spawnPoint);
                    //         break;
                    //     }
                    //     
                    //     cellPosition.y--;
                    }
                }
            }
            validPosition.Add(wave.name, w);
        }

        void Update() {
            UIManager.Instance.waveInfoBack.text = UIManager.Instance.waveInfo.text = "Remaining Monsters\n(" + Monster.monsters.Count + "/" + maxMonster + ")";
        }


        public void Event(WaveTrigger trigger) {

            
            if (trigger.data.monsters.Count <= 0 && Monster.monsters.Count <= 0) {
                trigger.complete = true;
            }

            if (colliders[colliders.IndexOf(trigger)].complete)
            {
                int idx = colliders.IndexOf(trigger);

                if (idx + 1 >= colliders.Count) return;
                colliders[idx + 1].renderer.enabled = false;
                colliders[idx + 1].GetComponent<Collider2D>().isTrigger = true;
            }
            
            trigger.currentDelay += Time.deltaTime;
            
            if (trigger.delay > trigger.currentDelay) return;
                        
            trigger.DelaySetup();
            List<Vector2> list = validPosition[trigger.name];
            Vector2 position = list[Mathf.FloorToInt(UnityEngine.Random.Range(0f, list.Count - 1))];

            if (trigger.data.monsters.Count <= 0) return;
            KeyValuePair<GameObject, int> kv = new KeyValuePair<GameObject, int>();
            
            
            foreach (KeyValuePair<GameObject, int> entry in trigger.data.monsters) {
                if (UnityEngine.Random.Range(0, 1) == 0 && entry.Key != null) kv = entry;
                if (kv.Key == null) kv = entry;
            }
            // Debug.Log("출발");

            GameObject obj = Instantiate(kv.Key, position, Quaternion.identity);
            Monster monster = obj.GetComponent<Monster>();
            monster.baseDamage += 5 * colliders.IndexOf(trigger);
            monster.SetHP(5 * colliders.IndexOf(trigger), 5 * colliders.IndexOf(trigger));

            int currentCount = kv.Value;

            maxMonster = Monster.monsters.Count;

            if (currentCount - 1 <= 0) trigger.data.monsters.Remove(kv.Key);
            else trigger.data.monsters[kv.Key] = currentCount - 1;
        }

        private void OnDestroy() {
            WaveSystem.Instance = null;
            
        }
    }
}