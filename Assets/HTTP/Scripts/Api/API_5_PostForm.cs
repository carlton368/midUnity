using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace HTTP
{
    public class API_5_PostForm : ApiBase
    {
        private static string Uri => $"{Common.Domain}/post-form";

        public static UnityWebRequest CreateWebRequestWithWWWForm(string name, int age)
        {
            var formData = new WWWForm();
            formData.AddField("name", name);
            formData.AddField("age", age + Random.Range(0, 10));
                
            var webRequest = UnityWebRequest.Post(Uri, formData);
            return webRequest;
        }
            
        public static UnityWebRequest CreateWebRequestWithMultipartForm(string name, int age)
        {
            var formData = new List<IMultipartFormSection>
            {
                new MultipartFormDataSection("name", name),
                new MultipartFormDataSection("age", $"{age + Random.Range(0, 10)}")
            };
                
            var webRequest = UnityWebRequest.Post(Uri, formData);
            return webRequest;
        }

        public class Result : ResultBase
        {
            public class Data
            {
                public string name;
                public int age;
            }

            public Data data;
        }
    }
}