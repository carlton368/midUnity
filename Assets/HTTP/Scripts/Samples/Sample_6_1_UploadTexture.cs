using System.Collections;
using UnityEngine;

namespace HTTP
{
    public class Sample_6_1_UploadTexture : Sample_Base
    {
        public Texture2D texture;
        
        public static string LatestUploadFilename;

        protected override IEnumerator RequestProcess()
        {
            using var webRequest = API_6_UploadFile.CreateWebRequest_withWWWForm(texture);
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