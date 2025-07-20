using TMPro;
using UnityEngine;

public class TextBehavior : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1.5f;
    [SerializeField] private float bounceHeight = .2f;
    [SerializeField] private float bounceSpeed = 2f;
    private TextMeshPro textMesh;
    private Color startColor;
    private float timer = 0f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
        textMesh = GetComponent<TextMeshPro>();
        startColor = textMesh.color;
    }

    void Update()
    {
        transform.position = startPos + new Vector3(0, Mathf.PingPong(Time.time * bounceSpeed, bounceHeight), 0);

        timer += Time.deltaTime;
        float alpha = Mathf.Lerp(startColor.a, 0f, timer / fadeDuration);

        textMesh.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

        if (timer >= fadeDuration)
        {
            Destroy(gameObject);
        }
    }
}
