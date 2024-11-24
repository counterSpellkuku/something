using UnityEngine;

namespace System.Wave
{
    public class Exit : MonoBehaviour {
        public void OnCollisionEnter2D(Collision2D other)
        {
            Debug.Log(other);
            if (other.gameObject.layer == LayerMask.NameToLayer("player")) {
                LoadingController.LoadScene("StartScene");
            }
        }
    }
}