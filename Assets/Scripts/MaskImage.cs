using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskImage : MonoBehaviour
{
    public float scaleChangeDuration = 2f;
    public float waitTime = 2f;

    RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        StartCoroutine(ScaleChange());
    }

    IEnumerator ScaleChange()
    {
        yield return new WaitForSeconds(2f);
        float startSize = 0.1f;
        float targetSize = 80f;
        float timeElapsed = 0f;

        while (timeElapsed < scaleChangeDuration)
        {
            // easeInOutCirc 함수를 사용하여 점진적으로 크기를 변경
            float newSize = easeInOutCirc(timeElapsed / scaleChangeDuration);

            // 크기 변경 (시작 스케일과 목표 스케일 사이의 값으로 보간)
            float scaledSize = Mathf.Lerp(startSize, targetSize, newSize);
            rect.localScale = new Vector3(scaledSize, scaledSize, 1f);

            // 경과 시간 업데이트
            timeElapsed += Time.deltaTime;

            yield return null;
        }
    }

    float easeInOutCirc(float x)
    {
        return 1 - MathF.Cos((x * 3.141592f) / 2);
        //return x < 0.5 ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * x, 2))) / 2 : (Mathf.Sqrt(1 - Mathf.Pow(-2 * x + 2, 2)) + 1) / 2;
    }
}

