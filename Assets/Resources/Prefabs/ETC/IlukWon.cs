using System.Collections;
using Entity;
using Entity.Player;
using UnityEngine;

public class IlukWon : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private GameObject dialogueSystemObject;
    [SerializeField] private DialogueSystem dialogueSystem;
    private Collider2D collider2D;
    private Animator _animator;
    
    private void Start()
    {
        collider2D = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
        if (collider2D == null)
        {
            Debug.LogError("Collider2D component is missing on this GameObject.");
        }
        if (_animator == null)
        {
            Debug.LogError("Animator component is missing on this GameObject.");
        }
    }

    private void OnMouseDown()
    {
        _animator.SetBool("Pressed",true);
        StartCoroutine(LoadSceneDelay());
    }

    IEnumerator LoadSceneDelay()
    {
        yield return new WaitForSeconds(1f);
        LoadingController.LoadScene("GameScene");
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (targetLayer == (targetLayer | (1 << other.gameObject.layer)))
        {
            OnTargetLayerOverlap(other.gameObject);
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (targetLayer == (targetLayer | (1 << other.gameObject.layer)))
        {
            dialogueSystemObject.SetActive(false);
        }
    }


    private void OnTargetLayerOverlap(GameObject targetObject)
    {
        dialogueSystemObject.SetActive(true);
        dialogueSystem.ShowDialogue(0);
        dialogueSystem.currentDialogueIndex = 0;
        Debug.Log($"Action triggered by {targetObject.name}");
        // 여기에서 실행할 동작을 정의하세요
        
    }
    
}