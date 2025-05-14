using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

namespace HTTP
{
    public class Sample_QuizAutoGenerate : MonoBehaviour
    {
        [SerializeField] private Button requestButton;
        [SerializeField] private TextMeshProUGUI resultText;
        
        private void Start()
        {
            if (requestButton != null)
                requestButton.onClick.AddListener(OnRequestButtonClicked);
        }
        
        private void OnRequestButtonClicked()
        {
            StartCoroutine(SendRequest());
        }
        
        private IEnumerator SendRequest()
        {
            if (resultText != null)
                resultText.text = "요청 전송 중...";
        
            var webRequest = API_QuizAutoGenerate.CreateWebRequest();
        
            Debug.Log($"요청 URL: {webRequest.uri}");
            Debug.Log($"요청 메서드: {webRequest.method}");
        
            yield return webRequest.SendWebRequest();
        
            Debug.Log($"응답 코드: {webRequest.responseCode}");
            Debug.Log($"서버 응답: {webRequest.downloadHandler.text}");
        
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                if (resultText != null)
                    resultText.text = "오류 발생: " + webRequest.error;
                yield break;
            }
        
            // 단순 문자열 응답 처리
            string responseMessage = webRequest.downloadHandler.text;
        
            if (resultText != null)
                resultText.text = "응답 메시지: " + responseMessage;
        
            Debug.Log("퀴즈 자동 생성 요청 성공: " + responseMessage);
        }
        
        private void OnDestroy()
        {
            if (requestButton != null)
                requestButton.onClick.RemoveListener(OnRequestButtonClicked);
        }
    }
}