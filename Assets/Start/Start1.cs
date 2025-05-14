using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Start1 : MonoBehaviour
{
    [SerializeField] private GameObject startMenu;
    [SerializeField] private Button startButton;

    void Start()
    {
        // 버튼 클릭 이벤트 연결
        startButton.onClick.AddListener(StartGame);
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

        SceneManager.LoadScene(nextSceneIndex);
    }
}
