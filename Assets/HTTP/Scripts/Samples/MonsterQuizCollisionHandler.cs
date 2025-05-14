using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace HTTP
{
    public class MonsterQuizTrigger : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            // 플레이어와 충돌했는지 확인
            if (collision.gameObject.CompareTag("Player"))
            {
                // 퀴즈 자동 생성 API 요청
                StartCoroutine(TriggerQuizGeneration());
            }
        }
        
        private IEnumerator TriggerQuizGeneration()
        {
            Debug.Log("몬스터와 충돌: 퀴즈 자동 생성 요청 시작");
            
            var webRequest = API_QuizAutoGenerate.CreateWebRequest();
            yield return webRequest.SendWebRequest();
            
            if (ApiBase.ErrorHandling(webRequest))
            {
                Debug.LogError("퀴즈 생성 요청 실패");
                yield break;
            }
            
            var result = ApiBase.GetResultFromJson<API_QuizAutoGenerate.Result>(webRequest);
            Debug.Log("퀴즈 생성 요청 성공: " + result.message);
            
            // 여기서 퀴즈 생성 후 최신 퀴즈를 가져오는 로직을 추가할 수 있습니다
            // 예: API_GetLatestQuiz를 호출하여 방금 생성된 퀴즈를 가져오기
        }
    }
}