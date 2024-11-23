using System;
using System.Collections.Generic;

namespace Entity.ViewAngle
{
    using UnityEngine;

    public class SectorCollider : MonoBehaviour
    {
        [Header("Sector Settings")]
        public float radius = 5f;
        [Range(0, 360)] public float angle = 90f;
        public int segments = 32;

        [Header("Visual Settings")] public Color sectorColor = new Color(1f, 1f, 0f, 0.3f);

        [Header("Layer Settings")] public LayerMask targetLayer;
        public LayerMask obstacleLayer;

        [Header("Debug")] public bool showDebug = true;

        private EdgeCollider2D edgeCollider;
        private List<Transform> detectedObjects = new List<Transform>();
        private SpriteRenderer spriteRenderer;
        private Sprite sectorSprite;
        private Texture2D sectorTexture;

        void Start() { InitializeComponents(); }

        void InitializeComponents()
        {
            // 콜라이더 초기화
            edgeCollider = GetComponent<EdgeCollider2D>();
            if (edgeCollider == null)
            {
                edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
            }

            edgeCollider.isTrigger = true;

            // 스프라이트 렌더러 초기화
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
                spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            Debug.Log(spriteRenderer);

            spriteRenderer.sortingOrder = -1;
            spriteRenderer.color = sectorColor;

            UpdateColliderAndSprite();
        }

        void UpdateColliderAndSprite() {
            
            int texSize = Mathf.NextPowerOfTwo(Mathf.RoundToInt(radius * 200));
            
            if (sectorTexture != null)
                DestroyImmediate(sectorTexture);
            
            sectorTexture = new Texture2D(texSize, texSize);
            sectorTexture.filterMode = FilterMode.Bilinear;

            
            Color[] colors = new Color[texSize * texSize];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = Color.clear;
            }

            sectorTexture.SetPixels(colors);

            
            Vector2 center = new Vector2(texSize / 2, texSize / 2);
            float pixelRadius = texSize / 2;
            float startAngle = -angle / 2f;
            float endAngle = angle / 2f;

            for (int x = 0; x < texSize; x++)
            {
                for (int y = 0; y < texSize; y++)
                {
                    Vector2 pixel = new Vector2(x - center.x, y - center.y);
                    float pixelAngle = Mathf.Atan2(pixel.x, pixel.y) * Mathf.Rad2Deg;
                    float pixelDist = pixel.magnitude;

                    if (pixelDist <= pixelRadius)
                    {
                        if (pixelAngle >= startAngle && pixelAngle <= endAngle)
                        {
                            sectorTexture.SetPixel(x, y, Color.white);
                        }
                    }
                }
            }

            sectorTexture.Apply();

            sectorSprite = Sprite.Create(
                sectorTexture,
                new Rect(0, 0, texSize, texSize),
                new Vector2(0.5f, 0.5f),
                100f
            );
            // Debug.Log(spriteRenderer);
            spriteRenderer.sprite = sectorSprite;

            
            UpdateColliderShape();
        }

        void UpdateColliderShape() {
            if (spriteRenderer == null) return;
            
            List<Vector2> points = new List<Vector2>();
            
            points.Add(Vector2.zero);

            float angleStep = angle / (segments - 1);
            float startAngle = -angle / 2;

            for (int i = 0; i <= segments; i++) {
                float currentAngle = startAngle + angleStep * i;
                float radian = currentAngle * Mathf.Deg2Rad;
                points.Add(new Vector2(Mathf.Sin(radian) * radius, Mathf.Cos(radian) * radius));
            }
            
            points.Add(Vector2.zero);
            
            edgeCollider.points = points.ToArray();
        }

        void OnValidate() {
            if (Application.isPlaying)
                UpdateColliderAndSprite();
        }

        void OnDestroy() {
            if (sectorTexture != null)
                DestroyImmediate(sectorTexture);
        }

        void OnTriggerStay2D(Collider2D other) {
            if (detectedObjects.Contains(other.transform)) return;
            
            if (!IsInLayerMask(other.gameObject.layer, targetLayer)) return;

            if (IsInViewAngle(other.transform))
            {
                if (!IsBlockedByObstacle(other.transform)) {
                    detectedObjects.Add(other.transform);
                    if (showDebug) Debug.Log($"Object Detected: {other.name}");
                }
            }
        }

        bool IsInViewAngle(Transform target)
        {
            Vector2 directionToTarget = (target.position - transform.position).normalized;
            float angleToTarget = Vector2.Angle(transform.up, directionToTarget);

            if (showDebug)
            {
                Debug.Log($"Angle to {target.name}: {angleToTarget}, Half View Angle: {angle / 2}");
                Debug.DrawRay(transform.position, directionToTarget * radius, Color.yellow, 0.1f);
            }

            return angleToTarget < angle / 2;
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (detectedObjects.Contains(other.transform))
            {
                detectedObjects.Remove(other.transform);
                if (showDebug) Debug.Log($"Object Lost: {other.name}");
            }
        }

        bool IsBlockedByObstacle(Transform target)
        {
            Vector2 dirToTarget = (target.position - transform.position).normalized;
            float distToTarget = Vector2.Distance(transform.position, target.position);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToTarget, distToTarget, obstacleLayer);

            if (showDebug && hit.collider != null)
            {
                Debug.Log($"Ray hit obstacle: {hit.collider.name}");
                Debug.DrawLine(transform.position, hit.point, Color.red, 0.1f);
            }

            return hit.collider != null;
        }

        bool IsInLayerMask(int layer, LayerMask layerMask) {
            return ((1 << layer) & layerMask) != 0;
        }

        public List<Transform> GetDetectedObjects()
        {
            return new List<Transform>(detectedObjects);
        }
    }
}