using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour
{
    public float shakeAmount = 0.1f;
    public float shakeDuration = 0.2f;

    private Vector3 originalPos;

    void Awake()
    {
        originalPos = transform.localPosition;
    }

    public void TriggerShake()
    {
        StopAllCoroutines(); // In case it's still shaking
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeAmount;
            float y = Random.Range(-1f, 1f) * shakeAmount;

            transform.localPosition = originalPos + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
