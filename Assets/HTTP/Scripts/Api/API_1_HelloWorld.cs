using UnityEngine.Networking;

namespace HTTP
{
    public class API_1_HelloWorld : ApiBase
    {
        private static string Uri => $"{Common.Domain}/hello-world";

        public static UnityWebRequest CreateWebRequest()
        {
            var webRequest = UnityWebRequest.Get(Uri);
            webRequest.SetRequestHeader("Content-Type", "text/plain");
            return webRequest;
        }

        public class Result : ResultBase
        {
            public string data;
        }
    }
}