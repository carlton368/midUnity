using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Start1 : MonoBehaviour
{
    [SerializeField] private GameObject startMenu;
    [SerializeField] private Button startButton;
    [SerializeField] private Image fadeImage; // 페이드 효과를 위한 이미지
    [SerializeField] private float fadeSpeed = 1.0f; // 페이드 속도

    void Start()
    {
        // 버튼 클릭 이벤트 연결
        startButton.onClick.AddListener(StartGame);
        
        // 페이드 이미지가 없으면 경고
        if (fadeImage == null)
        {
            Debug.LogWarning("페이드 이미지가 할당되지 않았습니다. 페이드 효과가 적용되지 않을 수 있습니다.");
        }
    }

    void StartGame()
    {
        // Start 메뉴 비활성화
        startMenu.SetActive(false);

        // 게임 시작 (씬 이동 or 타임스케일 복구)
        Time.timeScale = 1f;
        Debug.Log("게임 시작!");
    }

    public void LoadNextScene() 
    {
        // 현재 씬의 빌드 인덱스를 가져와서 다음 씬 로드
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // 만약 마지막 씬이면 첫 씬으로 돌아가기
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        // 비동기 씬 로드 및 페이드 효과 시작
        StartCoroutine(LoadSceneAsync(nextSceneIndex));
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        // 페이드 이미지가 없으면 그냥 씬 로드
        if (fadeImage == null)
        {
            SceneManager.LoadScene(sceneIndex);
            yield break;
        }

        // 페이드 아웃 (검은색으로 화면 가리기)
        fadeImage.gameObject.SetActive(true);
        Color currentColor = fadeImage.color;
        currentColor.a = 0f;
        fadeImage.color = currentColor;

        // 알파값을 1로 증가 (페이드 아웃)
        while (currentColor.a < 1.0f)
        {
            currentColor.a += fadeSpeed * Time.deltaTime;
            fadeImage.color = currentColor;
            yield return null;
        }

        // 비동기로 씬 로드
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        
        // 씬 로드가 완료될 때까지 대기
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // 참고: 새 씬에서는 페이드 인 효과를 별도로 구현해야 함
        // 이 스크립트는 씬이 전환되면 파괴되므로 새 씬에서 페이드 인 효과를 처리해야 함
    }
}