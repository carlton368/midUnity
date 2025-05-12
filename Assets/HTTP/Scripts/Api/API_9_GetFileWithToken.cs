using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace HTTP
{
    public class API_9_GetFileWithToken : ApiBase
    {
        private static string Uri => $"{Common.Domain}/get-file-with-token";

        public static UnityWebRequest CreateTextureWebRequest(string filename, string token)
        {
            var webRequest = UnityWebRequestTexture.GetTexture($"{Uri}/{filename}");
            webRequest.SetRequestHeader("Content-Type", "text/plain");
            webRequest.SetRequestHeader("Authorization", $"Bearer {token}");
            return webRequest;
        }
            
        public static UnityWebRequest CreateAudioClipWebRequest(string filename, string token)
        {
            var webRequest = UnityWebRequestMultimedia.GetAudioClip($"{Uri}/{filename}", AudioType.WAV);
            webRequest.SetRequestHeader("Content-Type", "text/plain");
            webRequest.SetRequestHeader("Authorization", $"Bearer {token}");
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