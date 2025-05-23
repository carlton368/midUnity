using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace HTTP
{
    public class Sample_2_GetJson : Sample_Base
    {
        protected override IEnumerator RequestProcess()
        {
            using var webRequest = API_2_GetJson.CreateWebRequest();
            
            // 요청 URL을 UI에 표시 (요청 전에 미리 표시)
            requestTextUI.text = webRequest.uri.ToString();
            
            yield return webRequest.SendWebRequest();

            if (ApiBase.ErrorHandling(webRequest))
            {
                responseTextUI.text = "오류가 발생했습니다.";
                yield break;
            }

            try
            {
                // 서버 응답 구조를 확인하기 위해 raw 텍스트로 처리
                string responseText = webRequest.downloadHandler.text;
                Debug.Log("서버 응답: " + responseText);
                
                // JObject로 파싱해서 구조 확인
                JObject jsonResponse = JObject.Parse(responseText);
                
                // 결과를 UI에 표시
                responseTextUI.text = $"퀴즈 ID: {jsonResponse["quizId"]}\n질문: {jsonResponse["question"]}";
            }
            catch (System.Exception ex)
            {
                Debug.LogError("JSON 파싱 오류: " + ex.Message);
                responseTextUI.text = "JSON 파싱 오류: " + ex.Message + "\n원본 응답: " + webRequest.downloadHandler.text;
            }
        }
    }
}