using TMPro;
using UnityEngine;
using System.Collections;

public class ShootingUIManager : MonoBehaviour
{
    //싱글톤 전역 변수
    public static ShootingUIManager Instance;
    [SerializeField] private GameObject killPanel;
    [SerializeField] private GameObject codePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    
    // 각 패널의 CanvasGroup
    private CanvasGroup killPanelCanvasGroup;
    private CanvasGroup codePanelCanvasGroup;
    private CanvasGroup winPanelCanvasGroup;
    private CanvasGroup losePanelCanvasGroup;
    
    // 페이드 효과 속도 조절 변수
    [SerializeField] private float fadeSpeed = 2.0f;
    
    public WinorLose WinorLoseState;
    public enum WinorLose
    {
        notsetted = 0,
        win = 1,
        lose = 2,
        code = 3,
    }
    
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        WinorLoseState = WinorLose.notsetted;
        
        // CanvasGroup 컴포넌트 가져오기 (없으면 추가)
        killPanelCanvasGroup = GetOrAddCanvasGroup(killPanel);
        codePanelCanvasGroup = GetOrAddCanvasGroup(codePanel);
        winPanelCanvasGroup = GetOrAddCanvasGroup(winPanel);
        losePanelCanvasGroup = GetOrAddCanvasGroup(losePanel);
        
        // 처음에는 모든 패널 비활성화 및 알파값 0으로 설정
        SetPanelInitialState(killPanel, killPanelCanvasGroup);
        SetPanelInitialState(codePanel, codePanelCanvasGroup);
        SetPanelInitialState(winPanel, winPanelCanvasGroup);
        SetPanelInitialState(losePanel, losePanelCanvasGroup);
    }
    
    private CanvasGroup GetOrAddCanvasGroup(GameObject panel)
    {
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = panel.AddComponent<CanvasGroup>();
        }
        return canvasGroup;
    }
    
    private void SetPanelInitialState(GameObject panel, CanvasGroup canvasGroup)
    {
        panel.SetActive(false);
        canvasGroup.alpha = 0f;
    }
    
    public void ShowWinPanel()
    {
        Debug.Log("Win");
        StartCoroutine(FadePanel(winPanel, winPanelCanvasGroup, true));
        //GameManager.Instance.AutoRestartGame();
    }
    
    public void ShowLosePanel()
    {
        Debug.Log("Lose");
        StartCoroutine(FadePanel(losePanel, losePanelCanvasGroup, true));
        //GameManager.Instance.AutoRestartGame();
    }
    
    public void ShowKillPanel()
    {
        Debug.Log("Kill");
        StartCoroutine(FadePanel(killPanel, killPanelCanvasGroup, true));
        //GameManager.Instance.AutoRestartGame();
    }
    
    public void CloseKillPanel()
    {
        Debug.Log("Close Kill");
        StartCoroutine(FadePanel(killPanel, killPanelCanvasGroup, false));
        //GameManager.Instance.AutoRestartGame();
    }
    
    public void ShowCodePanel()
    {
        Debug.Log("Code");
        StartCoroutine(FadePanel(codePanel, codePanelCanvasGroup, true));
        //GameManager.Instance.AutoRestartGame();
    }
    
    public void CloseCodePanel()
    {
        Debug.Log("Close Code");
        StartCoroutine(FadePanel(codePanel, codePanelCanvasGroup, false, DieOrNot));
        //GameManager.Instance.AutoRestartGame();
    }
    
    public void DieOrNot()
    {
        Debug.Log("Checking win/lose condition");
    }
    
    // 패널 페이드 인/아웃 코루틴
    private IEnumerator FadePanel(GameObject panel, CanvasGroup canvasGroup, bool fadeIn, System.Action onComplete = null)
    {
        // 페이드 인일 경우 패널 활성화
        if (fadeIn)
        {
            panel.SetActive(true);
        }
        
        float targetAlpha = fadeIn ? 1.0f : 0.0f;
        
        // 현재 알파값에서 목표 알파값까지 부드럽게 변경
        while (!Mathf.Approximately(canvasGroup.alpha, targetAlpha))
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, fadeSpeed * Time.deltaTime);
            yield return null;
        }
        
        // 페이드 아웃일 경우 패널 비활성화
        if (!fadeIn)
        {
            panel.SetActive(false);
        }
        
        // 완료 후 콜백 실행
        onComplete?.Invoke();
    }
}