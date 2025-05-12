using System.Collections;
using UnityEngine;

namespace HTTP
{
    public class Sample_6_2_UploadAudio : Sample_Base
    {
        public AudioClip audioClip;

        public static string LatestUploadFilename;
        
        protected override IEnumerator RequestProcess()
        {
            using var webRequest = API_6_UploadFile.CreateWebRequest_withWWWForm(audioClip);
            requestTextUI.text = webRequest.uri.ToString();
            yield return webRequest.SendWebRequest();

            if (ApiBase.ErrorHandling(webRequest))
            {
                yield break;
            }

            var result = ApiBase.GetResultFromJson<API_6_UploadFile.Result>(webRequest);
            responseTextUI.text = result.ToString();

            LatestUploadFilename = result.data.filename;
        }
    }
}