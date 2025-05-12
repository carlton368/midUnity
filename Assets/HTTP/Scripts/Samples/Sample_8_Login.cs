using System.Collections;

namespace HTTP
{
    public class Sample_8_Login : Sample_Base
    {
        public static string LatestAccessToken;
        
        protected override IEnumerator RequestProcess()
        {
            using var webRequest = API_8_Login.CreateWebRequest("testuser", "1234");
            requestTextUI.text = webRequest.uri.ToString();
            yield return webRequest.SendWebRequest();

            if (ApiBase.ErrorHandling(webRequest))
            {
                yield break;
            }

            var result = ApiBase.GetResultFromJson<API_8_Login.Result>(webRequest);
            responseTextUI.text = result.ToString();
            
            LatestAccessToken = result.data.access_token;
        }
    }
}