using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundFader : MonoBehaviour
{
    [SerializeField] private float fadeDuration;
    [SerializeField] private Image image;

    public void StartFading()
    {
        StartCoroutine(Fading());
    }

    private IEnumerator Fading()
    {
        float time = 0f;
        while (time <= 1f)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(1, 0, time));
            time += Time.deltaTime / fadeDuration;

            yield return null;
        }
    }
}
