using System.Collections;
using System.Collections.Generic;
using Entity;
using Entity.Monster;
using UnityEngine;

namespace Skills.Iceage
{
    public class IceageController : MonoBehaviour
    {
        [Header("Projectile Settings")]
        Projectile projectile;
    
    
        [Header("Fireball Settings")]
        [SerializeField] private GameObject iceblcokPrefab;
        [SerializeField] private Collider2D iceageCollider;
        [SerializeField] private Animator animator;
        [SerializeField] private LayerMask damageableLayer;
        private float damage;
        [SerializeField] private float freezeTime = 5f;
    
        private float fadeOutDuration = 1f; // 알파값을 줄이는 시간
        private SpriteRenderer spriteRenderer; // SpriteRenderer를 알파값 조정을 위해 추가
        private Vector3 initialDirection;
    
        private HashSet<GameObject> alreadyDamaged;
        
        public void Init(Vector3 target, float damage)
        {
            this.damage = damage;
        }
        
        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                Debug.LogError("SpriteRenderer not found! Please attach a SpriteRenderer to this GameObject.");
            }
            alreadyDamaged = new HashSet<GameObject>();
        }
        
        
        /// <summary>
        /// 애니메이션이 끝난 후 이벤트로 실행되는 함수
        /// </summary>
        public void FinishIceage()
        {
            StartCoroutine(DestroyAfterFadeOut());
        }
        
        private IEnumerator DestroyAfterFadeOut()
        {
            // 지진 지속 시간 대기
            yield return new WaitForSeconds(fadeOutDuration);

            // 데미지 판정 중단 (콜라이더 비활성화)
            if (iceageCollider != null)
            {
                iceageCollider.enabled = false;
            }

            // 페이드 아웃 처리
            if (spriteRenderer != null)
            {
                float elapsedTime = 0f;
                Color color = spriteRenderer.color;

                while (elapsedTime < fadeOutDuration)
                {
                    elapsedTime += Time.deltaTime;
                    float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
                    spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);
                    yield return null;
                }
            }

            // 오브젝트 삭제
            Destroy(gameObject);
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (((1 << collision.gameObject.layer) & damageableLayer) != 0)
            {
                var targetEntity = collision.GetComponent<Monster>();
                if (alreadyDamaged.Contains(targetEntity.gameObject)) return;
                if (targetEntity != null)
                {
                    alreadyDamaged.Add(targetEntity.gameObject);
                    
                    targetEntity.GetDamage(damage);
                    targetEntity.Stop();
                    targetEntity.stopMove = freezeTime;
                    
                    var iceblock = Instantiate(iceblcokPrefab, targetEntity.transform.position, Quaternion.identity);
                    iceblock.transform.SetParent(targetEntity.transform);
                    
                    IceblockController iceblockController = iceblock.GetComponent<IceblockController>();
                    iceblockController.StartCoroutine(iceblockController.DetroyAfterTime(freezeTime));
                }
            }
        }
    }
}