using System.Collections;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    [SerializeField] private float speed = 10f;
    private GameObject target;
    private static readonly int Explode = Animator.StringToHash("Explode");

    public void Init(GameObject target)
    {
        this.target = target;
        animator.SetTrigger("Fly");
    }

    private void Update()
    {
        if (target == null)
        {
            Debug.LogError("Fireball has no target. Destroying fireball.");
            Destroy(gameObject);
            return;
        }

        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.transform.position) < 0.5f)
        {
            StartCoroutine(OnHitTarget());
        }
    }

    private IEnumerator OnHitTarget()
    {
        Debug.Log($"Fireball hit {target.name}");
        animator.SetTrigger("Explode");
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
    
}