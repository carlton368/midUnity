using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace HTTP
{
    public class API_QuizSubmit : ApiBase
    {
        private static string Uri => $"{Common.Domain}/quiz/submit";

        public static UnityWebRequest CreateWebRequest(string quizId, string userAnswer)
        {
            var requestData = new Request
            {
                quizId = quizId,
                userAnswer = userAnswer
            };
            var body = JsonConvert.SerializeObject(requestData);
            
            // 수정된 부분: 올바른 방식으로 POST 요청 생성
            var webRequest = new UnityWebRequest(Uri, "POST");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(body);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            return webRequest;
        }
        
        public class Request : RequestBase
        {
            public string quizId;
            public string userAnswer;
        }

        public class Result : ResultBase
        {
            public string result;
        }
    }
}