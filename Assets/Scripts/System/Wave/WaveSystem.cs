using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Util;

namespace System.Wave
{
    
    public class WaveSystem : MonoBehaviour
    {

        public static WaveSystem Instance;
        [SerializeField] public List<WaveTrigger> colliders;
        [SerializeField] public SerializableDictionary<string, List<Vector2>> validPosition;

        
        public Tilemap groundTilemap;
        public LayerMask layer;

        
        
        public void Awake() {
            Instance = this;
        }

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



        public void Event(WaveTrigger trigger)
        {
            trigger.currentDelay += Time.deltaTime;

            if (trigger.delay > trigger.currentDelay) return;
            
            trigger.DelaySetup();
            List<Vector2> list = validPosition[trigger.name];
            Vector2 position = list[UnityEngine.Random.Range(0, list.Count)];

            if (trigger.data.monsters.Count <= 0) return;
            KeyValuePair<GameObject, int> kv = new KeyValuePair<GameObject, int>();
            
            
            foreach (KeyValuePair<GameObject, int> entry in trigger.data.monsters) {
                if (UnityEngine.Random.Range(0, 1) == 0 && entry.Key != null) kv = entry;
                if (kv.Key == null) kv = entry;
            }
            // Debug.Log("출발");

            Instantiate(kv.Key, position, Quaternion.identity);
            int currentCount = kv.Value;

            if (currentCount - 1 <= 0) trigger.data.monsters.Remove(kv.Key); 
        }

        private void OnDestroy() {
            WaveSystem.Instance = null;
            
        }
    }
}