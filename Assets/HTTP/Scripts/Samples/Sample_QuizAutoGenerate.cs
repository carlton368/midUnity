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
        [SerializeField] private TextMeshProUGUI inputText;
        private void Start()
        {
            if (requestButton != null)
                requestButton.onClick.AddListener(OnRequestButtonClicked);
        }
        
        private void OnRequestButtonClicked()
        {
            StartCoroutine(SendRequest());
        }

        public IEnumerator SendRequest()
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
        
            // 응답 처리
            string responseText = webRequest.downloadHandler.text;
            Debug.Log($"QuizAutoGenerate : {responseText}");
            
            // 퀴즈 ID 추출 시도
            if (API_QuizAutoGenerate.TryExtractQuizInfo(responseText, out string quizId, out string question))
            {
                // 퀴즈 ID가 추출되었으면 저장
                if (!string.IsNullOrEmpty(quizId))
                {
                    Common.LastQuizId = quizId;
                    Debug.Log($"새 퀴즈 생성됨, ID 업데이트: {quizId}");
                    
                    if (resultText != null)
                        resultText.text = $"새 퀴즈가 생성되었습니다.\n질문: {question}";
                }
            }
            else
            {
                // JSON 파싱 실패 또는 퀴즈 ID가 없음
                if (resultText != null)
                    resultText.text = "응답 메시지: " + responseText;
            }
            
            Debug.Log("퀴즈 자동 생성 요청 성공: " + responseText);
        }
        
        private void OnDestroy()
        {
            if (requestButton != null)
                requestButton.onClick.RemoveListener(OnRequestButtonClicked);
        }
        public void StartAutoExecution()
        {
            StartCoroutine(ExecuteAfterDelay());
        }
    
        private IEnumerator ExecuteAfterDelay()
        {
            Debug.Log("3초 후 자동 실행이 시작됩니다...");
            yield return new WaitForSeconds(3f);
            inputText.text = "a+b";
            Debug.Log("3초 후 자동 실행이 시작됩니다...");
            yield return new WaitForSeconds(3f);
        
            Sample_QuizSubmit quizGenerator = FindObjectOfType<Sample_QuizSubmit>();
            
            if (quizGenerator != null)
            {
                quizGenerator.SendRequest();
            }
            else
            {
                Debug.LogError("Sample_QuizSubmit 컴포넌트를 찾을 수 없습니다!");
            }
        }

    }
}