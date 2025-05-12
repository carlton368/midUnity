using UnityEngine.Networking;

namespace HTTP
{
    public class API_2_GetJson : ApiBase
    {
        private static string Uri => $"{Common.Domain}/get-json";

        public static UnityWebRequest CreateWebRequest()
        {
            var webRequest = UnityWebRequest.Get(Uri);
            webRequest.SetRequestHeader("Content-Type", "text/plain");
            return webRequest;
        }

        public class Result : ResultBase
        {
            public class Data
            {
                public int id;
                public string name;
                public string[] tags;
            }

            public Data data;
        }
    }
}