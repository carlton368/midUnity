using System.Collections;

namespace HTTP
{
    public class Sample_2_GetJson : Sample_Base
    {
        protected override IEnumerator RequestProcess()
        {
            using var webRequest = API_2_GetJson.CreateWebRequest();
            yield return webRequest.SendWebRequest();

            if (ApiBase.ErrorHandling(webRequest))
            {
                yield break;
            }
            
            requestTextUI.text = webRequest.uri.ToString();

            var result = ApiBase.GetResultFromJson<API_2_GetJson.Result>(webRequest);
            responseTextUI.text = result.ToString();
        }
    }
}