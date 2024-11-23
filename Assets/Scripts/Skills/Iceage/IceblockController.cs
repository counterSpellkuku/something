using System.Collections;
using UnityEngine;

public class IceblockController : MonoBehaviour
{
    public IEnumerator DetroyAfterTime(float freezeTime)
    {
        yield return new WaitForSeconds(freezeTime);
        Destroy(gameObject);
    }
}
