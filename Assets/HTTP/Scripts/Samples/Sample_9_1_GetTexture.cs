using System.Collections;
using UnityEngine.UI;

namespace HTTP
{
    public class Sample_9_1_GetTexture : Sample_Base
    {
        public Image image;

        protected override IEnumerator RequestProcess()
        {
            using var webRequest = API_9_GetFileWithToken.CreateTextureWebRequest(
                Sample_6_1_UploadTexture.LatestUploadFilename,
                Sample_8_Login.LatestAccessToken);
            requestTextUI.text = webRequest.uri.ToString();
            yield return webRequest.SendWebRequest();

            if (ApiBase.ErrorHandling(webRequest))
            {
                yield break;
            }
            
            image.sprite = API_9_GetFileWithToken.GetSprite(webRequest);
            responseTextUI.text = $"이미지 다운로드 성공\n{image.sprite.name} ({webRequest.downloadedBytes:N0}bytes)";
        }
    }
}