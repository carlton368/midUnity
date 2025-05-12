using System.Collections;

namespace HTTP
{
    public class Sample_1_HelloWorld : Sample_Base
    {
        protected override IEnumerator RequestProcess()
        {
            using var webRequest = API_1_HelloWorld.CreateWebRequest();
            requestTextUI.text = webRequest.uri.ToString();
            yield return webRequest.SendWebRequest();

            if (ApiBase.ErrorHandling(webRequest))
            {
                yield break;
            }

            var result = ApiBase.GetResultFromJson<API_1_HelloWorld.Result>(webRequest);
            responseTextUI.text = result.ToString();
        }
    }
}