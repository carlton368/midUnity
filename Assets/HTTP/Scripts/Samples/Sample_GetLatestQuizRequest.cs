using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace HTTP
{
    public class Sample_GetLatestQuizRequest : Sample_Base
    {
        protected override IEnumerator RequestProcess()
        {
            using var webRequest = API_GetLatestQuiz.CreateWebRequest();
            
            // 요청 URL을 UI에 표시
            requestTextUI.text = webRequest.uri.ToString();
            Debug.Log("!!!!!!!!!!!!!!!!!!!!!Sample_GetLatestQuizRequest");
            
            // 요청 보내기
            yield return webRequest.SendWebRequest();

            Debug.Log($"서버 응답 코드: {webRequest.responseCode}");
            
            // 성공 여부 확인 후 처리
            if (webRequest.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                Debug.Log($"GetLatestQuizRequest : {webRequest.downloadHandler.text}");
                
                try
                {
                    // JSON에서 결과 파싱
                    var jsonText = webRequest.downloadHandler.text;
                    JObject jsonObj = JObject.Parse(jsonText);
                    
                    string quizId = jsonObj["quizId"]?.ToString();
                    string question = jsonObj["question"]?.ToString();
                    
                    // 결과를 UI에 표시
                    responseTextUI.text = $"퀴즈 ID: {quizId}\n질문: {question}";
                    
                    // 중요: 최신 퀴즈 ID를 Common 클래스에 저장
                    if (!string.IsNullOrEmpty(quizId))
                    {
                        Common.LastQuizId = quizId;
                        Debug.Log($"최신 퀴즈 ID 업데이트: {Common.LastQuizId}");
                    }
                    else
                    {
                        Debug.LogWarning("서버에서 받은 퀴즈 ID가 비어 있습니다.");
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("JSON 파싱 오류: " + ex.Message);
                    responseTextUI.text = "JSON 파싱 오류: " + ex.Message + "\n원본 응답: " + webRequest.downloadHandler.text;
                }
            }
            else
            {
                // 오류 발생시 자세한 정보 로깅
                Debug.LogError($"서버 오류: {webRequest.error}, 상태 코드: {webRequest.responseCode}");
                
                if (!string.IsNullOrEmpty(webRequest.downloadHandler.text))
                {
                    Debug.LogError($"오류 응답 내용: {webRequest.downloadHandler.text}");
                }
                
                // 사용자에게 메시지 표시
                responseTextUI.text = $"서버 오류가 발생했습니다.\n코드: {webRequest.responseCode}\n기본 퀴즈 ID를 사용합니다.";
                
                // 기본 퀴즈 ID 유지 (Common.LastQuizId 프로퍼티는 이미 기본값을 반환함)
                Debug.Log($"기본 퀴즈 ID 유지: {Common.LastQuizId}");
            }
        }
    }
}