using System.Collections;

namespace HTTP
{
    public class Sample_5_1_PostWWWForm : Sample_Base
    {
        protected override IEnumerator RequestProcess()
        {
            using var webRequest = API_5_PostForm.CreateWebRequestWithWWWForm("호두", 7);
            requestTextUI.text = webRequest.uri.ToString();
            yield return webRequest.SendWebRequest();

            if (ApiBase.ErrorHandling(webRequest))
            {
                yield break;
            }
            
            var result = ApiBase.GetResultFromJson<API_5_PostForm.Result>(webRequest);
            responseTextUI.text = result.ToString();
        }
    }
}