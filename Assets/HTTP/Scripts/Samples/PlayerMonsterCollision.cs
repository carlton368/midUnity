using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTTP;

public class PlayerMonsterCollision : MonoBehaviour
{
    // 충돌 진입 감지 (트리거 콜라이더 사용 시)
    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 객체가 "Monster" 태그를 가지고 있는지 확인
        if (other.CompareTag("Monster"))
        {
            // 현재 오브젝트가 "Player" 태그를 가지고 있는지 확인
            if (gameObject.CompareTag("Player"))
            {
                // Player와 Monster 태그를 가진 객체가 충돌했을 때 SendRequest 메소드 호출
                HTTP.Sample_GetLatestQuizRequest GetLatestQuizRequest = FindObjectOfType<HTTP.Sample_GetLatestQuizRequest>();
                if (GetLatestQuizRequest != null)
                {
                    GetLatestQuizRequest.SendRequest();
                    Debug.Log("Player와 Monster가 충돌했습니다. GetLatestQuizRequest 메소드가 호출되었습니다.");
                }
                else
                {
                    Debug.LogError("GetLatestQuizRequest 컴포넌트를 찾을 수 없습니다.");
                }
            }
        }
    }
}