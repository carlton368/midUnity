using System.Collections;
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
            
            // 요청 보내기
            yield return webRequest.SendWebRequest();

            var a = webRequest.downloadHandler.text;
            Debug.Log($"서버 응답: {webRequest.downloadHandler.text}");

            // 오류 처리
            if (ApiBase.ErrorHandling(webRequest))
            {
                responseTextUI.text = "오류가 발생했습니다.";
                yield break;
            }

            try
            {
                // JSON에서 결과 파싱
                var result = ApiBase.GetResultFromJson<API_GetLatestQuiz.Result>(webRequest);
                
                // 결과를 UI에 표시
                responseTextUI.text = result.ToString();
                
                // 디버그 로그 추가
                //Debug.Log($"최신 퀴즈: ID={result.quizId}, 질문={result.question}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError("JSON 파싱 오류: " + ex.Message);
                responseTextUI.text = "JSON 파싱 오류: " + ex.Message + "\n원본 응답: " + webRequest.downloadHandler.text;
            }
        }
    }
}