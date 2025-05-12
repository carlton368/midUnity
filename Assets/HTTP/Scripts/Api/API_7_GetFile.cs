using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace HTTP
{
    public class API_7_GetFile : ApiBase
    {
        private static string Uri => $"{Common.Domain}/get-file";

        public static UnityWebRequest CreateTextureWebRequest(string filename)
        {
            var webRequest = UnityWebRequestTexture.GetTexture($"{Uri}/{filename}");
            webRequest.SetRequestHeader("Content-Type", "text/plain");
            return webRequest;
        }
            
        public static UnityWebRequest CreateAudioClipWebRequest(string filename)
        {
            var webRequest = UnityWebRequestMultimedia.GetAudioClip($"{Uri}/{filename}", AudioType.WAV);
            webRequest.SetRequestHeader("Content-Type", "text/plain");
            return webRequest;
        }

        public static Sprite GetSprite(UnityWebRequest webRequest)
        {
            // 응답 데이터를 Texture2D로 복원
            var texture = DownloadHandlerTexture.GetContent(webRequest);
            var sprite = Sprite.Create(texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
            sprite.name = Path.GetFileName(webRequest.url);
            return sprite;
        }

        public static AudioClip GetAudioClip(UnityWebRequest webRequest)
        {
            var audioClip = DownloadHandlerAudioClip.GetContent(webRequest);
            audioClip.name = Path.GetFileName(webRequest.url);
            return audioClip;
        }
    }
}