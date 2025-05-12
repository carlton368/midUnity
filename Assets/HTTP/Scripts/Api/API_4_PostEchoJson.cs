using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace HTTP
{
    public class API_4_PostEchoJson : ApiBase
    {
        private static string Uri => $"{Common.Domain}/post-echo-json";

        public static UnityWebRequest CreateWebRequest(string name, int age)
        {
            var requestData = new Request
            {
                name = name,
                age = age + Random.Range(0, 10),
            };
            var body = JsonConvert.SerializeObject(requestData);
            var webRequest = UnityWebRequest.Post(Uri, body, UnityWebRequest.kHttpVerbPOST);
            webRequest.SetRequestHeader("Content-Type", "application/json");
            return webRequest;
        }
        
        public class Request : RequestBase
        {
            public string name;
            public int age;
        }

        public class Result : ResultBase
        {
            public class Data
            {
                public class Result
                {
                    public string name;
                    public int age;
                }
                
                public Result received_json;
            }

            public Data data;
        }
    }
}