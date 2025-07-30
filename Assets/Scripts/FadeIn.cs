using UnityEngine;

public class FadeIn : MonoBehaviour
{
    public float fadeDuration = 2f;

    private SpriteRenderer sr;
    private float fadeTimer = 0f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            return;
        }

        Color color = sr.color;
        color.a = 0f;
        sr.color = color;
    }

    void Update()
    {
        if (sr == null) return;

        if (fadeTimer < fadeDuration)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Clamp01(fadeTimer / fadeDuration);

            Color color = sr.color;
            color.a = alpha;
            sr.color = color;
        }
    }
}