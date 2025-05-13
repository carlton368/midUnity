using System.Collections;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace HTTP
{
    public class Sample_QuizSubmit : Sample_Base
    {
        protected override IEnumerator RequestProcess()
        {
            // 타임아웃 설정을 조정하여 POST 요청 생성
            string uri = "http://192.168.0.154:8080/quiz-ai/quiz";
            var webRequest = new UnityWebRequest(uri, "POST");
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            
            // 빈 JSON 객체를 요청 본문에 추가
            string jsonData = "{}";
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            
            webRequest.SetRequestHeader("Content-Type", "application/json");
            
            // 타임아웃 10초로 설정 (밀리초 단위)
            webRequest.timeout = 10;
            
            requestTextUI.text = $"요청 URL: {webRequest.uri.ToString()}";
            Debug.Log($"요청 시작: {uri}");
            
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || 
                webRequest.result == UnityWebRequest.Result.ProtocolError ||
                webRequest.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.LogError($"API 요청 실패: {webRequest.error}");
                responseTextUI.text = $"오류 발생: {webRequest.error}";
                yield break;
            }

            // 서버 응답 구조를 확인하기 위해 raw 텍스트로 처리
            string responseText = webRequest.downloadHandler.text;
            Debug.Log("서버 응답: " + responseText);
            
            try
            {
                // 일단 결과를 직접 표시
                responseTextUI.text = responseText;
                
                if (string.IsNullOrEmpty(responseText))
                {
                    Debug.LogWarning("서버 응답이 비어있습니다");
                    responseTextUI.text = "서버 응답이 비어있습니다";
                    yield break;
                }
                
                // JObject로 파싱해서 구조 확인
                JObject jsonResponse = JObject.Parse(responseText);
                string prettyJson = jsonResponse.ToString(Formatting.Indented);
                Debug.Log("응답 구조: \n" + prettyJson);
                
                // 퀴즈 정보 파싱 예시
                string quizId = jsonResponse["quizId"].ToString();
                string question = jsonResponse["question"].ToString();
                
                Debug.Log($"퀴즈 ID: {quizId}, 질문: {question}");
                responseTextUI.text = $"퀴즈: {question} (ID: {quizId})";
                
                // correctAnswer는 백엔드에서만 사용되고 프론트에는 응답하지 않음
            }
            catch (System.Exception ex)
            {
                Debug.LogError("JSON 파싱 오류: " + ex.Message);
                responseTextUI.text = "JSON 파싱 오류: " + ex.Message;
            }
        }
    }
}