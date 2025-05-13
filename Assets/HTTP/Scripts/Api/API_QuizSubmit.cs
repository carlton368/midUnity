using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace HTTP
{
    public class API_QuizSubmit : ApiBase
    {
        private static string Uri => "http://192.168.0.154:8080/quiz-ai/quiz";

        public static UnityWebRequest CreateWebRequest()
        {
            // 빈 JSON 객체 생성
            string jsonData = "{}";
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            
            // POST 요청 생성 - 빈 JSON 객체 전송
            var webRequest = new UnityWebRequest(Uri, "POST");
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.timeout = 15;
            return webRequest;
        }

        public class Result : ResultBase
        {
            // 서버에서 직접 반환하는 필드들
            public string quizId;
            public string question;
            // correctAnswer는 백엔드에서만 사용되고, 프론트에는 응답하지 않음
        }
    }
}