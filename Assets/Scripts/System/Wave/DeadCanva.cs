using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace System.Wave
{
    public class DeadCanva: MonoBehaviour
    {

        
        [Header("UI References")]
    [SerializeField] private Image gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button mainMenuButton;
    
    [Header("Animation Settings")]
    [SerializeField] private float fadeInDuration = 1.5f;
    [SerializeField] private float textScaleUpDuration = 1.0f;
    [SerializeField] private float buttonsFadeInDelay = 0.5f;
    
    private void Start()
    {
        if (gameOverPanel != null)
        {
            Color color = gameOverPanel.color;
            color.a = 0;
            gameOverPanel.color = color;
            
            gameOverPanel.gameObject.SetActive(false);
        }
        
        if (retryButton != null)
            retryButton.onClick.AddListener(RetryGame);
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(GoToMainMenu);
    }
    
    public void ShowGameOver()
    {
        gameOverPanel.gameObject.SetActive(true);
        StartCoroutine(GameOverSequence());
    }
    
    private IEnumerator GameOverSequence()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            Color color = gameOverPanel.color;
            color.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);
            gameOverPanel.color = color;

            yield return null;
        }
        
        // Game Over 텍스트 애니메이션
        if (gameOverText != null)
        {
            gameOverText.transform.localScale = Vector3.zero;
            elapsedTime = 0f;
            
            while (elapsedTime < textScaleUpDuration)
            {
                elapsedTime += Time.deltaTime;
                float scale = Mathf.Lerp(0f, 1f, elapsedTime / textScaleUpDuration);
                gameOverText.transform.localScale = new Vector3(scale, scale, scale);
                yield return null;
            }
        }
        
        // 버튼 페이드 인
        yield return new WaitForSeconds(buttonsFadeInDelay);
        
        if (retryButton != null)
            retryButton.gameObject.SetActive(true);
        if (mainMenuButton != null)
            mainMenuButton.gameObject.SetActive(true);

        Time.timeScale = 0.3f;
    }
    
    public void RetryGame()
    {
        // 현재 씬 다시 로드
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );

        Time.timeScale = 1;
    }
    
    public void GoToMainMenu()
    {
        LoadingController.LoadScene("StartScene");
        Time.timeScale = 1;
         }
    }
}