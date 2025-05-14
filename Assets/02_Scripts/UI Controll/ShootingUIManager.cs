using TMPro;
using UnityEngine;

public class ShootingUIManager : MonoBehaviour
{
    //싱글톤 전역 변수
    public static ShootingUIManager Instance;
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
        Debug.Log("Win");
        winPanel.SetActive(true);
        //GameManager.Instance.AutoRestartGame();
    }
    
    public void ShowLosePanel()
    {
        Debug.Log("Lose");
        losePanel.SetActive(true);
        //GameManager.Instance.AutoRestartGame();
    }
    
    public void ShowCodePanel()
    {
        Debug.Log("Code");
        codePanel.SetActive(true);
        //GameManager.Instance.AutoRestartGame();
    }
    
    public void CloseCodePanel()
    {
        Debug.Log("Code");
        codePanel.SetActive(false);
        DieOrNot();
        //GameManager.Instance.AutoRestartGame();
    }
    public void DieOrNot()
    {
        Debug.Log("Checking win/lose condition");
    }
}
