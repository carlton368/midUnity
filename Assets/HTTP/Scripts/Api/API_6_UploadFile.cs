using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace HTTP
{
    public class API_6_UploadFile : ApiBase
    {
        private static string Uri => $"{Common.Domain}/upload-file";

#region WWWForm
        public static UnityWebRequest CreateWebRequest_withWWWForm(Texture2D texture)
        {
            var bytes = texture.EncodeToPNG();
            
            var formData = new WWWForm();
            formData.AddBinaryData("file", bytes, $"{texture.name}.png", "image/png");
            
            var webRequest = UnityWebRequest.Post(Uri, formData);
            return webRequest;
        }
        
        public static UnityWebRequest CreateWebRequest_withWWWForm(AudioClip audioClip)
        {
            var bytes = Common.AudioClipToWav(audioClip);
            
            var formData = new WWWForm();
            formData.AddBinaryData("file", bytes, $"{audioClip.name}.wav", "audio/wav");
            
            var webRequest = UnityWebRequest.Post(Uri, formData);
            return webRequest;
        }
#endregion

#region MultipartForm
        public static UnityWebRequest CreateWebRequest_withMultipartForm(Texture2D texture)
        {
            var bytes = texture.EncodeToPNG();

            var formData = new List<IMultipartFormSection>
            {
                new MultipartFormFileSection("file", bytes, $"{texture.name}.png", "image/png"),
            };
            
            var webRequest = UnityWebRequest.Post(Uri, formData);
            return webRequest;
        }
        
        public static UnityWebRequest CreateWebRequest_withMultipartForm(AudioClip audioClip)
        {
            var bytes = Common.AudioClipToWav(audioClip);

            var formData = new List<IMultipartFormSection>
            {
                new MultipartFormFileSection("file", bytes, $"{audioClip.name}.wav", "audio/wav"),
            };
            
            var webRequest = UnityWebRequest.Post(Uri, formData);
            return webRequest;
        }
 #endregion
 
        public class Result : ResultBase
        {
            public class Data
            {
                public string filename;
                public int filesize;
            }

            public Data data;
        }
    }
}