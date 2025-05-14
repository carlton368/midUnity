using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;

namespace HTTP
{
    public class Sample_GetLatestQuizRequest : Sample_Base
    {
        public float delayTime = 3f;
        public string responseText = "";
        public TMP_Text textUI;
        
        protected override IEnumerator RequestProcess()
        {
            using var webRequest = API_GetLatestQuiz.CreateWebRequest();
            
            // 요청 URL을 UI에 표시
            requestTextUI.text = webRequest.uri.ToString();
            Debug.Log("!!!!!!!!!!!!!!!!!!!!!Sample_GetLatestQuizRequest");
            
            // 요청 보내기
            yield return webRequest.SendWebRequest();

            Debug.Log($"서버 응답 코드: {webRequest.responseCode}");
            
            // 성공 여부 확인 후 처리
            if (webRequest.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                Debug.Log($"GetLatestQuizRequest : {webRequest.downloadHandler.text}");
                
                try
                {
                    // JSON에서 결과 파싱
                    var jsonText = webRequest.downloadHandler.text;
                    JObject jsonObj = JObject.Parse(jsonText);
                    
                    string quizId = jsonObj["quizId"]?.ToString();
                    string question = jsonObj["question"]?.ToString();
                    
                    // 결과를 UI에 표시
                    responseTextUI.text = $"질문 : \n{question}";
                    Invoke(nameof(DelayedCall), delayTime);
                    
                    // 중요: 최신 퀴즈 ID를 Common 클래스에 저장
                    if (!string.IsNullOrEmpty(quizId))
                    {
                        Common.LastQuizId = quizId;
                        Debug.Log($"최신 퀴즈 ID 업데이트: {Common.LastQuizId}");
                    }
                    else
                    {
                        Debug.LogWarning("서버에서 받은 퀴즈 ID가 비어 있습니다.");
                    }
                    
                    //Invoke(nameof(MonsterDie), delayTime);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("JSON 파싱 오류: " + ex.Message);
                    responseTextUI.text = "JSON 파싱 오류: " + ex.Message + "\n원본 응답: " + webRequest.downloadHandler.text;
                }
            }
            else
            {
                // 오류 발생시 자세한 정보 로깅
                Debug.LogError($"서버 오류: {webRequest.error}, 상태 코드: {webRequest.responseCode}");
                
                if (!string.IsNullOrEmpty(webRequest.downloadHandler.text))
                {
                    Debug.LogError($"오류 응답 내용: {webRequest.downloadHandler.text}");
                }
                
                // 사용자에게 메시지 표시
                responseTextUI.text = $"서버 오류가 발생했습니다.\n코드: {webRequest.responseCode}\n기본 퀴즈 ID를 사용합니다.";
                
                // 기본 퀴즈 ID 유지 (Common.LastQuizId 프로퍼티는 이미 기본값을 반환함)
                Debug.Log($"기본 퀴즈 ID 유지: {Common.LastQuizId}");
            }
        }

        private void DelayedCall()
        {
            textUI.text = responseText;
            //Invoke(nameof(PlayerDie), delayTime);
            Invoke(nameof(MonsterDie), delayTime);
        }
        
        private void PlayerDie()
        {
            ShootingUIManager.Instance.CloseCodePanel();
            ShootingUIManager.Instance.ShowLosePanel();
        }
        private void MonsterDie()
        {
            ShootingUIManager.Instance.CloseCodePanel();
            ShootingUIManager.Instance.ShowKillPanel();
            Invoke("CallCloseKillPanel", delayTime);
            
            // 씬에 있는 모든 Monster 객체 찾기
            GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
            
            // 각 몬스터에 대해 페이드 아웃 효과 적용
            foreach (GameObject monster in monsters)
            {
                StartCoroutine(FadeOutMonster(monster, 2.0f));
            }
        }

        // 몬스터 페이드 아웃 코루틴
        private IEnumerator FadeOutMonster(GameObject monster, float duration)
        {
            if (monster == null) yield break;
            
            // 몬스터의 모든 렌더러 컴포넌트 가져오기
            Renderer[] renderers = monster.GetComponentsInChildren<Renderer>();
            
            // 애니메이터 비활성화 (애니메이션 중지)
            Animator animator = monster.GetComponent<Animator>();
            if (animator != null)
            {
                animator.enabled = false;
            }
            
            // 몬스터 AI 비활성화 (움직임 중지)
            MonsterAI_NoNavMesh monsterAI = monster.GetComponent<MonsterAI_NoNavMesh>();
            if (monsterAI != null)
            {
                monsterAI.enabled = false;
            }
            
            float elapsedTime = 0f;
            
            // 기존 머티리얼과 색상 저장
            Material[][] originalMaterials = new Material[renderers.Length][];
            Color[][] originalColors = new Color[renderers.Length][];
            
            for (int i = 0; i < renderers.Length; i++)
            {
                originalMaterials[i] = renderers[i].materials;
                originalColors[i] = new Color[originalMaterials[i].Length];
                
                for (int j = 0; j < originalMaterials[i].Length; j++)
                {
                    // 원래 색상 저장
                    originalColors[i][j] = originalMaterials[i][j].color;
                }
            }
            
            // 시간에 따라 알파값 감소
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Clamp01(1 - (elapsedTime / duration));
                
                // 모든 렌더러의 모든 머티리얼 투명도 조정
                for (int i = 0; i < renderers.Length; i++)
                {
                    for (int j = 0; j < originalMaterials[i].Length; j++)
                    {
                        Color newColor = originalColors[i][j];
                        newColor.a = alpha;
                        originalMaterials[i][j].color = newColor;
                    }
                }
                
                yield return null;
            }
            
            // 페이드 아웃 완료 후 몬스터 제거
            Destroy(monster);
        }

        private void CallCloseKillPanel()
        {
            ShootingUIManager.Instance.CloseKillPanel();
        }
    }
}