using TMPro;
using UnityEngine;

public class ShootingUIManager : MonoBehaviour
{
    //싱글톤 전역 변수
    public static ShootingUIManager Instance { get; set; }
    [SerializeField] private GameObject codePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
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
    }
    public void ShowWinPanel()
    {
        if (WinorLoseState != WinorLose.win)
            return;
        Debug.Log("Win");
        winPanel.SetActive(true);
        //GameManager.Instance.AutoRestartGame();
    }
    
    public void ShowLosePanel()
    {
        if (WinorLoseState != WinorLose.lose)
            return;
        Debug.Log("Lose");
        losePanel.SetActive(true);
        //GameManager.Instance.AutoRestartGame();
    }
    
    public void ShowCodePanel()
    {
        if (WinorLoseState != WinorLose.code)
            return;
        Debug.Log("Code");
        losePanel.SetActive(true);
        //GameManager.Instance.AutoRestartGame();
    }
    private void Update()
    {
    }
}
