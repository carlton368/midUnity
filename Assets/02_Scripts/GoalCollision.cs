using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTTP;

public class GoalCollision : MonoBehaviour
{
    // 충돌 진입 감지 (트리거 콜라이더 사용 시)
    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 객체가 "Monster" 태그를 가지고 있는지 확인
        if (other.CompareTag("Goal"))
        {
            // 현재 오브젝트가 "Player" 태그를 가지고 있는지 확인
            if (gameObject.CompareTag("Player"))
            {
                ShootingUIManager.Instance.ShowWinPanel();
                Debug.Log("Win!!!!!!!!");
            }
        }
    }
}