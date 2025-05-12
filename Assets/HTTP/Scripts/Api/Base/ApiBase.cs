using UnityEngine;
using UnityEngine.Networking;

namespace HTTP
{
    public class ApiBase
    {
        public static bool ErrorHandling(UnityWebRequest request)
        {
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"{request.uri} 실패\n{request.error}\n{request.downloadHandler.text}");
                return true;
            }

            return false;
        }

        public static T GetResultFromJson<T>(UnityWebRequest webRequest) where T : ResultBase
        {
            var resultText = webRequest.downloadHandler.text;
            var result = ResultBase.FromJson<T>(resultText);
            return result;
        }
    }
}