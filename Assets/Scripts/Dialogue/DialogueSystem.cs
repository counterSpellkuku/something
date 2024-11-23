using System;
using TMPro; // TextMeshPro 사용
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueSystem : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject dialogPanel; // 다이얼로그 패널
    [SerializeField] private Image leftImage; // 상대방 일러스트
    [SerializeField] private Image rightImage; // 본인 일러스트
    [SerializeField] private TextMeshProUGUI dialogText; // 대사 텍스트

    [Header("Dialogue Data")]
    [SerializeField] private Sprite leftSprite; // 상대방 일러스트
    [SerializeField] private Sprite rightSprite; // 본인 일러스트
    [SerializeField] private string[] dialogues; // 대사 리스트
    [SerializeField] private bool[] isLeftSpeaker; // 왼쪽이 말하는지 여부
    [SerializeField] private float typingSpeed = 0.05f; // 타이핑 효과 속도

    public int currentDialogueIndex = 0;
    private Coroutine typingCoroutine;
    
    private void Start()
    {
        // if (dialogues.Length > 0)
        // {
        //     ShowDialogue(currentDialogueIndex);
        // }
        // else
        // {
        //     Debug.LogWarning("No dialogues available!");
        // }
    }

    private void Update()
    {
        // Space 키 입력으로 다음 대사로 넘어감
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextDialogue();
        }
    }

    public void ShowDialogue(int index)
    {
        if (index < 0 || index >= dialogues.Length)
        {
            Debug.LogWarning("Dialogue index out of range!");
            return;
        }

        // 화자에 따라 이미지 설정
        if (isLeftSpeaker[index])
        {
            leftImage.sprite = leftSprite;
            leftImage.gameObject.SetActive(true);
            rightImage.gameObject.SetActive(false);
        }
        else
        {
            rightImage.sprite = rightSprite;
            rightImage.gameObject.SetActive(true);
            leftImage.gameObject.SetActive(false);
        }

        // 타이핑 효과를 적용하여 텍스트 설정
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeText(dialogues[index]));
    }

    private IEnumerator TypeText(string text)
    {
        dialogText.text = ""; // 텍스트 초기화
        foreach (char c in text)
        {
            dialogText.text += c;
            yield return new WaitForSeconds(typingSpeed); // 한 글자씩 출력
        }
    }

    public void NextDialogue()
    {
        currentDialogueIndex++;
        if (currentDialogueIndex < dialogues.Length)
        {
            ShowDialogue(currentDialogueIndex);
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        dialogPanel.SetActive(false);
        Debug.Log("Dialogue ended.");
    }
}
