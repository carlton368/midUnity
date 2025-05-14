using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace HTTP
{
    public class Sample_QuizSubmit : Sample_Base
    {
        private string userAnswer = "test흑"; // 테스트용 기본값

        // 하드코딩된 퀴즈 정보 (서버에서 제공된 예시와 일치시킴)
        private string quizId = "8ff7dfe6-b5bd-43e7-b9c6-6e5e3392b1bd";

        protected override IEnumerator RequestProcess()
        {
            // 디버그 확인을 위한 이전 값 저장
            string previousQuizId = quizId;
            
            // Common.last_quizId 값 확인 - 비어있으면 기본값 유지
            if (!string.IsNullOrEmpty(Common.last_quizId))
            {
                quizId = Common.last_quizId;
            }
            
            // quizId가 비어있는지 최종 확인
            if (string.IsNullOrEmpty(quizId))
            {
                Debug.LogWarning("last_quizId가 비어있어서 기본값으로 복원합니다.");
                quizId = previousQuizId; // 초기 하드코딩된 값으로 복원
            }
            
            Debug.Log($"퀴즈 ID 확인: Common.last_quizId=[{Common.last_quizId}], 최종 quizId=[{quizId}]");
            
            // 요청 정보 로깅
            Debug.Log($"퀴즈 제출: ID={quizId}, 답변={userAnswer}");
            requestTextUI.text = $"POST {Common.Domain}/quiz/submit\n" +
                                $"퀴즈 ID: {quizId}\n" +
                                $"답변: {userAnswer}";

            // 퀴즈 답변 제출 요청 생성
            using var submitRequest = API_QuizSubmit.CreateWebRequest(quizId, userAnswer);
            
            // 요청 시작 전 원본 요청 데이터 로깅
            var requestData = new API_QuizSubmit.Request
            {
                quizId = quizId,
                userAnswer = userAnswer
            };
            
            Debug.Log("요청 데이터: " + JsonConvert.SerializeObject(requestData));
            
            // 요청 보내기
            yield return submitRequest.SendWebRequest();

            // 오류 처리 (중요: 응답 코드 확인)
            if (submitRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"HTTP 오류: {submitRequest.error}, 상태 코드: {submitRequest.responseCode}");
                responseTextUI.text = $"오류 발생: {submitRequest.error}\n";
                
                // 응답 내용이 있으면 표시
                if (!string.IsNullOrEmpty(submitRequest.downloadHandler.text))
                {
                    responseTextUI.text += "서버 응답: " + submitRequest.downloadHandler.text;
                }
                
                yield break;
            }

            // 정상 응답 처리
            string submitResponseText = submitRequest.downloadHandler.text;
            Debug.Log("서버 응답: " + submitResponseText);
            
            try
            {
                // 응답이 비어있는지 확인
                if (string.IsNullOrEmpty(submitResponseText))
                {
                    responseTextUI.text = "서버 응답이 비어있습니다.";
                    yield break;
                }
                
                // JSON 파싱
                JObject jsonResponse = JObject.Parse(submitResponseText);
                
                // 결과 표시
                string result = jsonResponse["result"]?.ToString();
                responseTextUI.text = result ?? "알 수 없는 응답";
                
                // 로깅
                Debug.Log("퀴즈 결과: " + result);
            }
            catch (System.Exception ex)
            {
                Debug.LogError("JSON 파싱 오류: " + ex.Message);
                responseTextUI.text = "JSON 파싱 오류: " + ex.Message + "\n원본 응답: " + submitResponseText;
            }
        }
    }
}