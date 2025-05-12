using System.Collections;

namespace HTTP
{
    public class Sample_3_GetEchoText : Sample_Base
    {
        protected override IEnumerator RequestProcess()
        {
            using var webRequest = API_3_GetEchoText.CreateWebRequest("send message");
            requestTextUI.text = webRequest.uri.ToString();
            yield return webRequest.SendWebRequest();

            if (ApiBase.ErrorHandling(webRequest))
            {
                yield break;
            }

            var result = ApiBase.GetResultFromJson<API_3_GetEchoText.Result>(webRequest);
            responseTextUI.text = result.ToString();
        }
    }
}