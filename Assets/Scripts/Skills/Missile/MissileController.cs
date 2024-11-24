using System.Collections;
using System.Collections.Generic;
using System.Manager;
using Entity;
using UnityEngine;

namespace Skills.Missile
{
    public class MissileController : MonoBehaviour
    {
        [Header("Missile Settings")]
        [SerializeField] private Animator animator;
        [SerializeField] private float baseSpeed = 10f; // 초기 속도
        [SerializeField] private float maxSpeedMultiplier = 3f; // 최대 속도 배율
        [SerializeField] private float accelerationTime = 2f; // 속도 증가 시간
        [SerializeField] private float explosionDistance = 1f;
        [SerializeField] private float explosionRadius = 2f; // 광역 데미지 반경
        [SerializeField] private float rotationSpeed = 200f; // 회전 속도 (각도/초)
        [SerializeField] private LayerMask damageableLayer; // 데미지를 받을 레이어
        [SerializeField] private float explosionDelay = 0.5f; // 폭발 후 지속 시간

        private GameObject target;
        private float damage;
        private HashSet<GameObject> alreadyDamaged = new HashSet<GameObject>();
        private bool hasExploded = false;
        private float currentSpeed; // 현재 속도
        private float accelerationTimer = 0f; // 가속 시간 타이머

        public void Init(GameObject target, float damage)
        {
            this.target = target;
            this.damage = damage;
            currentSpeed = baseSpeed; // 초기 속도 설정

            if (target != null)
            {
                Vector3 direction = (target.transform.position - transform.position).normalized;
                float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, targetAngle);
            }

            CamManager.main.Shake(0.3f, 0.5f);
        }

        private void Update()
        {
            if (hasExploded) return;

            Accelerate();
            MoveTowardsTarget();
        }

        private void Accelerate()
        {
            // 가속 로직: accelerationTime 동안 속도를 선형적으로 증가
            if (accelerationTimer < accelerationTime)
            {
                accelerationTimer += Time.deltaTime;
                float t = Mathf.Clamp01(accelerationTimer / accelerationTime); // 0에서 1까지 선형 증가
                currentSpeed = Mathf.Lerp(baseSpeed, baseSpeed * maxSpeedMultiplier, t);
            }
        }

        private void MoveTowardsTarget()
        {
            if (target == null) return;

            // 타겟 방향 계산
            Vector3 direction = (target.transform.position - transform.position).normalized;

            // 미사일 회전
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, targetAngle), rotationSpeed * Time.deltaTime);

            // 이동
            transform.position += direction * currentSpeed * Time.deltaTime;

            // 타겟 근처에서 폭발
            if (Vector3.Distance(transform.position, target.transform.position) <= explosionDistance)
            {
                Explode();
            }
        }

        private void Explode()
        {
            if (hasExploded) return;

            hasExploded = true;
            currentSpeed = 0f;
            animator?.SetTrigger("Explode");
            ApplyExplosionDamage();
        }

        private void ApplyExplosionDamage()
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, damageableLayer);
            foreach (Collider2D collider in hitColliders)
            {
                if (!alreadyDamaged.Add(collider.gameObject)) continue; // 중복 검사

                var entity = collider.GetComponent<BaseEntity>();
                if (entity != null)
                {
                    entity.GetDamage(damage);
                }
            }
        }

        public void DestroyThis()
        {
            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            // 폭발 반경 시각화
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}
