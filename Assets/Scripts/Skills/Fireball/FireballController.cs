using System;
using System.Collections;
using Entity;
using UnityEngine;



/// <summary>
/// 사용하기 위해서는 꼭! Projectile 컴포넌트를 추가해주세요!
/// 사용하기 위해서는 꼭! 폭발 애니메이션 끝에 DestroyThis()를 호출해주세요!
/// </summary>
public class FireballController : MonoBehaviour
{
    [Header("Projectile Settings")]
    Projectile projectile;
    
    
    [Header("Fireball Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private float maxTravelTime = 3f;
    [SerializeField] private float damage;
    
    private float fadeOutDuration = 1f; // 알파값을 줄이는 시간
    private SpriteRenderer spriteRenderer; // SpriteRenderer를 알파값 조정을 위해 추가
    private bool canFly = true;
    private float currentTravelTime = 0f;
    private Vector3 initialDirection;

    public void Init(GameObject target, float damage)
    {
        // 초기 방향 계산
        Vector3 direction = (target.transform.position - transform.position).normalized;
        initialDirection = direction;
        this.damage = damage;

        // Fireball의 방향을 조정
        RotateTowardsDirection(direction);

        projectile = GetComponent<Projectile>();
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found! Please attach a SpriteRenderer to this GameObject.");
        }
    }

    private void Update()
    {
        if (!canFly) return;
        
        currentTravelTime += Time.deltaTime;
        
        if (currentTravelTime >= maxTravelTime)
        {
            canFly = false;
            StartCoroutine(FadeOutAndDestroy());
            return;
        }

        // Fireball 이동
        transform.position += initialDirection * speed * Time.deltaTime;

        // 타겟에 충돌 처리
        foreach (Transform target in projectile.targets) {
            print(target.name);
            var entity = target.GetComponent<BaseEntity>();
            if (entity != null)
            {
                entity.GetDamage(10);
            }
        }
    }
    

    private void RotateTowardsDirection(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
    
    /// <summary>
    /// 어디에 부딪힌 경우 Explode 애니메이션 이벤트에서 작동합니다~
    /// </summary>
    public void DestroyThis()
    {
        Destroy(gameObject);
    }

    private IEnumerator FadeOutAndDestroy()
    {
        Debug.Log("Fireball reached max distance. Fading out.");

        float elapsedTime = 0f;
        Color originalColor = spriteRenderer.color;

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        Destroy(gameObject);
    }
}
