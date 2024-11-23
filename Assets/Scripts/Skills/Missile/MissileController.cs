using System.Collections;
using Entity;
using UnityEngine;

namespace Skills.Missile
{
    /// <summary>
    /// 사용하기 위해서는 꼭! Projectile 컴포넌트를 추가해주세요!
    /// 사용하기 위해서는 꼭! 폭발 애니메이션 끝에 DestroyThis()를 호출해주세요!
    /// </summary>
    public class MissileController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private float speed = 10f;
        [SerializeField] private float explosionRadius = 1f; // 광역 데미지 반경
        [SerializeField] private float rotationSpeed = 200f; // 회전 속도 (각도/초)

        private float damage;
        private GameObject target;

        private Projectile projectile;
        [SerializeField] private bool canFly = true;
        [SerializeField] private bool hasExploded = false; // 폭발 여부 체크

        public void Init(GameObject target, float damage)
        {
            this.target = target;
            this.damage = damage;
            projectile = GetComponent<Projectile>();
        }

        private void Update()
        {
            // 폭발하거나 비행 중지 시 이동 로직 정지
            if (hasExploded || !canFly) return;

            if (target == null)
            {
                Debug.LogError($"{name} has no target. Destroying missile.");
                Destroy(gameObject);
                return;
            }

            // 타겟 방향 계산
            Vector3 direction = (target.transform.position - transform.position).normalized;

            // 미사일이 타겟 방향을 바라보도록 회전
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float currentAngle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 0, currentAngle);

            // 타겟 방향으로 이동
            transform.position += direction * speed * Time.deltaTime;

            // 타겟 리스트 순회
            foreach (Transform target in projectile.targets)
            {
                // 충돌 후 폭발
                if (!hasExploded) // 추가 방어로직
                {
                    Explode(); // 폭발 처리
                    break; // 첫 번째 충돌 시 루프 종료
                }
            }
        }

        private void Explode()
        {
            if (hasExploded) return; // 이미 폭발한 경우 중단
            hasExploded = true; // 폭발 처리 플래그 설정
            canFly = false; // 비행 중지

            // 이동 중지 (현재 위치 유지)
            speed = 0f;



            // 광역 데미지 적용
            ApplyAoEDamage();
        }

        private void ApplyAoEDamage()
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            foreach (Collider2D hitCollider in hitColliders)
            {
                var aoeEntity = hitCollider.GetComponent<BaseEntity>();
                if (aoeEntity != null)
                {
                    aoeEntity.GetDamage(damage);
                }
            }
        }

        /// <summary>
        /// 폭발 반경 시각화를 위한 Gizmos
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            // 폭발 반경을 시각화
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);

            // Collider2D로 탐지된 오브젝트들을 시각화
            if (Application.isPlaying)
            {
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
                foreach (Collider2D collider in hitColliders)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(collider.transform.position, 0.1f); // 탐지된 오브젝트의 위치 표시
                }
            }
        }

        /// <summary>
        /// 애니메이션 이벤트에서 호출
        /// </summary>
        public void DestroyThis()
        {
            Destroy(gameObject);
        }
    }
}
