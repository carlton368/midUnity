using UnityEngine;
using UnityEngine.Networking;
namespace HTTP
{
    public class API_QuizAutoGenerate : ApiBase
    {
        private static string Uri => $"{Common.Domain}/quiz-auto/generate";
        
        public static UnityWebRequest CreateWebRequest()
        {
            WWWForm form = new WWWForm();
            var webRequest = UnityWebRequest.Post(Uri, form);
            webRequest.SetRequestHeader("Content-Type", "application/json");
            return webRequest;
        }
        
        // 문자열 응답을 처리하기 위한 메서드
        public static string GetStringResult(UnityWebRequest webRequest)
        {
            return webRequest.downloadHandler.text;
        }
        
        // 기존 클래스는 유지하되 사용하지 않음
        public class Result : ResultBase
        {
            public string message;
        }
    }
}