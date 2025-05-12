using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace HTTP
{
    public class Sample_9_2_GetAudio : Sample_Base
    {
        public AudioSource audioSource;

        protected override IEnumerator RequestProcess()
        {
            using var webRequest = API_9_GetFileWithToken.CreateAudioClipWebRequest(
                Sample_6_2_UploadAudio.LatestUploadFilename,
                Sample_8_Login.LatestAccessToken);
            requestTextUI.text = webRequest.uri.ToString();
            yield return webRequest.SendWebRequest();

            if (ApiBase.ErrorHandling(webRequest))
            {
                yield break;
            }
            
            var audioClip = API_9_GetFileWithToken.GetAudioClip(webRequest);
            audioSource.Stop();
            audioSource.clip = audioClip;
            audioSource.Play();
            responseTextUI.text = $"오디오클립 다운로드 성공\n{audioClip.name} ({webRequest.downloadedBytes:N0}bytes)";
        }
    }
}