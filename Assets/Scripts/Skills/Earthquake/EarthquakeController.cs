using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEngine;

namespace Skills.Earthquake
{
    public class EarthquakeController : MonoBehaviour
    {
        [SerializeField] private Animator animator; // 지진 애니메이션
        [SerializeField] private float damage = 10f; // 데미지
        [SerializeField] private float duration = 3f; // 지진 전체 지속 시간
        [SerializeField] private LayerMask damageableLayer; // 데미지를 받을 레이어
        [SerializeField] private float fadeOutDuration = 1f; // 페이드 아웃 시간
        

        private SpriteRenderer spriteRenderer; // 스프라이트 렌더러
        private Collider2D earthquakeCollider; // 지진의 충돌 처리
        private HashSet<GameObject> alreadyDamaged;
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                Debug.LogError("EarthquakeController requires a SpriteRenderer on the same GameObject!");
            }

            earthquakeCollider = GetComponent<Collider2D>();
            if (earthquakeCollider == null)
            {
                Debug.LogError("EarthquakeController requires a Collider2D on the same GameObject!");
            }

            alreadyDamaged = new HashSet<GameObject>();
        }
        
        public void Init(float damage)
        {
            this.damage = damage; // 데미지 설정
            

            
        }
        
        
        /// <summary>
        /// 애니메이션 끝에서 실행되게 해주세요.
        /// </summary>
        public void FinishEarthquake()
        {
            StartCoroutine(DestroyAfterFadeOut());
        }
        
        
        private IEnumerator DestroyAfterFadeOut()
        {
            // 지진 지속 시간 대기
            yield return new WaitForSeconds(duration);

            // 데미지 판정 중단 (콜라이더 비활성화)
            if (earthquakeCollider != null)
            {
                earthquakeCollider.enabled = false;
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
                float knockForce;
                var targetEntity = collision.GetComponent<BaseEntity>();
                if (alreadyDamaged.Contains(targetEntity.gameObject)) return;
                if (targetEntity != null)
                {
                    knockForce = Random.Range(10f, 20f);
                    targetEntity.KnockBack(Vector3.zero, knockForce, 0.1f);
                    print(targetEntity.GetDamage(damage));
                    alreadyDamaged.Add(targetEntity.gameObject);
                }
            }
        }
    }
}
