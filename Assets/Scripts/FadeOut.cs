using Unity.VisualScripting;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public float fadeDuration = 2f;
    private SpriteRenderer sr;
    private float fadeTimer = 0f;
    private Color originalColor;
    private bool resetComplete = true;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = true;
        originalColor = sr.color;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnReset.AddListener(Reset);
        }
    }

    void Reset()
    {
        sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
        fadeTimer = 0f;
    }

    void Update()
    {
        if (fadeTimer < fadeDuration)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Lerp(originalColor.a, 0f, fadeTimer / fadeDuration);
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
        }
    }

}
