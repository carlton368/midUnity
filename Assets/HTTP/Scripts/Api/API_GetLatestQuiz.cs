using UnityEngine.Networking;

namespace HTTP
{
    public class API_GetLatestQuiz : ApiBase
    {
        private static string Uri => $"{Common.Domain}/quiz/latest";

        public static UnityWebRequest CreateWebRequest()
        {
            var webRequest = UnityWebRequest.Get(Uri);
            webRequest.SetRequestHeader("Content-Type", "application/json");
            return webRequest;
        }

        public class Result : ResultBase
        {
            public string quizId;
            public string question;
        }
    }
}