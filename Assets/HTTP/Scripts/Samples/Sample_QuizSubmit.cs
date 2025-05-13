using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace HTTP
{
    public class Sample_QuizSubmit : Sample_Base
    {
        protected override IEnumerator RequestProcess()
        {
            string quizId = "q002";
            string question = "자니?";
            string userAnswer = "큐";

            using var webRequest = API_QuizSubmit.CreateWebRequest(quizId, question, userAnswer);
            requestTextUI.text = webRequest.uri.ToString();
            yield return webRequest.SendWebRequest();

            if (ApiBase.ErrorHandling(webRequest))
            {
                yield break;
            }

            // 서버 응답 구조를 확인하기 위해 raw 텍스트로 처리
            string responseText = webRequest.downloadHandler.text;
            Debug.Log("서버 응답: " + responseText);
            
            try
            {
                // 일단 결과를 직접 표시
                responseTextUI.text = responseText;
                
                // JObject로 파싱해서 구조 확인
                JObject jsonResponse = JObject.Parse(responseText);
                string prettyJson = jsonResponse.ToString(Formatting.Indented);
                Debug.Log("응답 구조: \n" + prettyJson);
                
                // 구조가 확인되면 이후 API_QuizSubmit.Result 클래스를 수정할 것
            }
            catch (System.Exception ex)
            {
                Debug.LogError("JSON 파싱 오류: " + ex.Message);
                responseTextUI.text = "JSON 파싱 오류: " + ex.Message;
            }
        }
    }
}