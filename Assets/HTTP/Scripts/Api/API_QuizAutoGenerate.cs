using Newtonsoft.Json;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace HTTP
{
    public class API_QuizAutoGenerate : ApiBase
    {
        private static string Uri => $"{Common.Domain}/quiz/prepare";
        
        public static UnityWebRequest CreateWebRequest()
        {
            // 빈 JSON 객체 생성 (서버가 요구할 경우)
            var emptyJsonBody = "{}";
            var webRequest = new UnityWebRequest(Uri, "POST");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(emptyJsonBody);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            return webRequest;
        }
        
        // 문자열 응답을 처리하기 위한 메서드
        public static string GetStringResult(UnityWebRequest webRequest)
        {
            return webRequest.downloadHandler.text;
        }
        
        // 퀴즈 ID와 질문을 추출하는 메서드 (추가)
        public static bool TryExtractQuizInfo(string responseText, out string quizId, out string question)
        {
            quizId = null;
            question = null;
            
            try
            {
                var jsonObj = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(responseText);
                quizId = jsonObj?["quizId"]?.ToString();
                question = jsonObj?["question"]?.ToString();
                
                return !string.IsNullOrEmpty(quizId);
            }
            catch (Exception ex)
            {
                //Debug.LogError($"퀴즈 정보 추출 오류: {ex.Message}");
                return false;
            }
        }
        
        // 응답 클래스
        public class Result : ResultBase
        {
            public string quizId;
            public string question;
            
            public override string ToString()
            {
                return $"퀴즈 ID: {quizId}\n질문: {question}";
            }
        }
    }
}