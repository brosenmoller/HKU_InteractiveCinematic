using System.Collections;
using UnityEngine;

public class LightFading : MonoBehaviour
{
    [SerializeField] private float fadeDuration;
    [SerializeField] private Light _light;

    private float targetIntensity;
    private float targetSize;

    public void Awake()
    {
        targetIntensity = _light.intensity;
        targetSize = transform.localScale.x;

        _light.intensity = 0;
        transform.localScale = Vector3.zero;
    }

    public void StartFading()
    {
        StartCoroutine(Fading());
    }

    private IEnumerator Fading()
    {
        float time = 0f;
        while (time <= 1f)
        {
            time += Time.deltaTime / fadeDuration;

            _light.intensity = Mathf.Lerp(0, targetIntensity, time);
            float newSize = Mathf.Lerp(0, targetSize, time);

            transform.localScale = new Vector3(newSize, newSize, newSize);

            yield return null;
        }
    }
}
