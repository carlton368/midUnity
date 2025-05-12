using System.Collections;

namespace HTTP
{
    public class Sample_0_UndefinedApi : Sample_Base
    {
        protected override IEnumerator RequestProcess()
        {
            using var webRequest = API_0_UndefinedApi.CreateWebRequest();
            requestTextUI.text = webRequest.uri.ToString();
            yield return webRequest.SendWebRequest();

            if (ApiBase.ErrorHandling(webRequest))
            {
                yield break;
            }

            var result = ApiBase.GetResultFromJson<API_0_UndefinedApi.Result>(webRequest);
            responseTextUI.text = result.ToString();
        }
    }
}