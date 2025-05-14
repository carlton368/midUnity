using System;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI_NoNavMesh : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    public float stopDistance = 2f;
    public float rotationSpeed = 5f;
    public Animator animator;

    void Update()
    {
        // 플레이어와의 거리 계산
        float distance = Vector3.Distance(transform.position, player.position);

        // 일정 거리 이상이면 따라가기
        if (distance > stopDistance)
        {
            // 플레이어 방향 계산
            Vector3 direction = (player.position - transform.position).normalized;

            // 몬스터 회전 (부드럽게)
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // 몬스터 이동
            transform.position += direction * speed * Time.deltaTime;
        }
        
        animator.SetBool("Near", distance <= stopDistance);
    }

    private void OnAnimatorMove()
    {
        
    }
}
