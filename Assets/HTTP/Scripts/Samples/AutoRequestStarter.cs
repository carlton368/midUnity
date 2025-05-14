using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTTP;

public class AutoRequestStarter : MonoBehaviour
{
    // 모든 Start() 메서드가 실행된 후 호출될 지연 시간 (초)
    public float delayAfterStart = 0.5f;

    // Start 메서드는 첫 프레임 업데이트 전에 호출됩니다
    private void Start()
    {
        // 코루틴을 시작하여 약간의 지연 후 SendRequest 호출
        StartCoroutine(CallSendRequestAfterAllStarts());
    }

    // 모든 Start() 메서드가 실행된 후 SendRequest를 호출하는 코루틴
    private IEnumerator CallSendRequestAfterAllStarts()
    {
        // 모든 Start() 메서드가 완료되도록 약간의 지연 시간을 줍니다 
        yield return new WaitForSeconds(delayAfterStart);

        // Sample_QuizAutoGenerate 컴포넌트 찾기
        Sample_QuizAutoGenerate quizAutoGenerate = FindObjectOfType<Sample_QuizAutoGenerate>();

        // 컴포넌트가 존재하면 SendRequest 메서드 호출
        if (quizAutoGenerate != null)
        {
            Debug.Log("모든 Start() 메서드 실행 후 Sample_QuizAutoGenerate.SendRequest() 호출");
            StartCoroutine(quizAutoGenerate.SendRequest());
        }
        else
        {
            Debug.LogError("Sample_QuizAutoGenerate 컴포넌트를 찾을 수 없습니다!");
        }
    }
}