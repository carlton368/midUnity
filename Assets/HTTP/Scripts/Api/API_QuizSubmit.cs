using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace HTTP
{
    public class API_QuizSubmit : ApiBase
    {
        private static string Uri => "http://192.168.0.154:8080/quiz/submit";

        public static UnityWebRequest CreateWebRequest(string quizId, string question, string userAnswer)
        {
            var requestData = new Request
            {
                quizId = quizId,
                question = question,
                userAnswer = userAnswer
            };
            var body = JsonConvert.SerializeObject(requestData);
            var webRequest = UnityWebRequest.Post(Uri, body, UnityWebRequest.kHttpVerbPOST);
            webRequest.SetRequestHeader("Content-Type", "application/json");
            return webRequest;
        }
        
        public class Request : RequestBase
        {
            public string quizId;
            public string question;
            public string userAnswer;
        }

        public class Result : ResultBase
        {
            // 서버에서 직접 반환하는 필드들
            public string quizId;
            public string question;
            public string userAnswer;
            public bool isCorrect;
        }
    }
}