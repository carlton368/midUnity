using UnityEngine.Networking;

namespace HTTP
{
    public class API_3_GetEchoText : ApiBase
    {
        private static string Uri => $"{Common.Domain}/get-echo-text";

        public static UnityWebRequest CreateWebRequest(string message)
        {
            var webRequest = UnityWebRequest.Get($"{Uri}?message={message}");
            webRequest.SetRequestHeader("Content-Type", "text/plain");
            return webRequest;
        }

        public class Result : ResultBase
        {
            public class Data
            {
                public string received_message;
            }

            public Data data;
        }
    }
}