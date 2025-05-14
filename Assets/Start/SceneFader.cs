using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeSpeed = 1.0f;

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        if (fadeImage == null)
        {
            yield break;
        }

        // 검은 화면에서 시작
        Color currentColor = fadeImage.color;
        currentColor.a = 1f; // 완전 불투명
        fadeImage.color = currentColor;
        fadeImage.gameObject.SetActive(true);

        // 알파값을 0으로 감소 (페이드 인)
        while (currentColor.a > 0.0f)
        {
            currentColor.a -= fadeSpeed * Time.deltaTime;
            fadeImage.color = currentColor;
            yield return null;
        }

        // 페이드 완료 후 이미지 비활성화
        fadeImage.gameObject.SetActive(false);
    }
}