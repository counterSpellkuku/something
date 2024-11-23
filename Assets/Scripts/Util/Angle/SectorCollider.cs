using System;
using System.Collections.Generic;

namespace Entity.ViewAngle
{
    using UnityEngine;

    public class SectorCollider : MonoBehaviour 
    {
        public float radius = 5f;
        [Range(0, 360)]
        public float angle = 90f;
        public int segments = 50;
    
        public LayerMask targetLayer; // 감지할 대상의 레이어
        public LayerMask obstacleLayer; // 장애물 레이어
    
        private PolygonCollider2D polygonCollider;
        private List<Transform> detectedObjects = new List<Transform>();
        void Start()
        {
            polygonCollider = GetComponent<PolygonCollider2D>();
            if (polygonCollider == null)
            {
                polygonCollider = gameObject.AddComponent<PolygonCollider2D>();
            }
            polygonCollider.isTrigger = true;
        
            UpdateColliderShape();
        }
    
        void OnTriggerEnter2D(Collider2D other)
        {
            // Debug.Log(IsInLayerMask(other.gameObject.layer, targetLayer));
            if (IsInLayerMask(other.gameObject.layer, targetLayer))
            {
                if (!IsBlockedByObstacle(other.transform))
                {
                    detectedObjects.Add(other.transform);
                    Debug.Log(other.name);
                }
            }
        }
    
        void OnTriggerExit2D(Collider2D other)
        {
            if (detectedObjects.Contains(other.transform))
            {
                detectedObjects.Remove(other.transform);
                Debug.Log(other.name);
            }
        }
        void UpdateColliderShape()
        {
            Vector2[] points = new Vector2[segments + 2];
            points[0] = Vector2.zero;
        
            float startAngle = -angle / 2f;
            float angleStep = angle / segments;
        
            for (int i = 0; i <= segments; i++) {
                float currentAngle = startAngle + angleStep * i;
                float rad = currentAngle * Mathf.Deg2Rad;
            
                float x = Mathf.Sin(rad) * radius;
                float y = Mathf.Cos(rad) * radius;
            
                points[i + 1] = new Vector2(x, y);
            }
        
            polygonCollider.points = points;
        }


        
        bool IsBlockedByObstacle(Transform target)
        {
            Vector2 dirToTarget = (target.position - transform.position).normalized;
            float distToTarget = Vector2.Distance(transform.position, target.position);
        
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToTarget, distToTarget, obstacleLayer);
            return hit.collider != null;
        }
    
        public List<Transform> GetDetectedObjects()
        {
            return new List<Transform>(detectedObjects);
        }
    
        bool IsInLayerMask(int layer, LayerMask layerMask) {
            return ((1 << layer) & layerMask) != 0;
        }
    
        void OnValidate()
        {
            if (polygonCollider != null) 
                UpdateColliderShape();
            
        }
        void OnDrawGizmos()
        {
            
            Gizmos.color = Color.yellow;
            Vector3 position = transform.position;
            float startAngle = -angle / 2f;
            float angleStep = angle / segments;
        
            for (int i = 0; i <= segments; i++)
            {
                float currentAngle = startAngle + angleStep * i;
                float rad = currentAngle * Mathf.Deg2Rad;
            
                Vector3 direction = new Vector3(Mathf.Sin(rad), Mathf.Cos(rad), 0);
                Gizmos.DrawLine(position, position + direction * radius);
            }
        
            Gizmos.color = Color.red;
            foreach (Transform detected in detectedObjects)
            {
                if (detected != null)
                {
                    Gizmos.DrawLine(position, detected.position);
                    Gizmos.DrawWireSphere(detected.position, 0.3f);
                }
            }
        }

        public void OnTriggerStay2D(Collider2D other) {
            Debug.Log(other);
        }
    }
}