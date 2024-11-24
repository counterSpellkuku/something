using System;
using UnityEngine;

public class GotoEnd : MonoBehaviour
{
    
    [SerializeField] private LayerMask targetLayer;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (targetLayer == (targetLayer | (1 << other.gameObject.layer))) LoadingController.LoadScene("EndCredits");
    }
}
