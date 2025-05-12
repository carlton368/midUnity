using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace HTTP
{
    public class API_8_Login : ApiBase
    {
        private static string Uri => $"{Common.Domain}/login";

        public static UnityWebRequest CreateWebRequest(string username, string password)
        {
            var requestData = new Request
            {
                username = username,
                password = password,
            };
            var body = JsonConvert.SerializeObject(requestData);
            var webRequest = UnityWebRequest.Post(Uri, body, UnityWebRequest.kHttpVerbPOST);
            webRequest.SetRequestHeader("Content-Type", "application/json");
            return webRequest;
        }
        
        public class Request : RequestBase
        {
            public string username;
            public string password;
        }

        public class Result : ResultBase
        {
            public class Data
            {
                public string access_token;
            }

            public Data data;
        }
    }
}