using UnityEngine;

public class OnPicture : MonoBehaviour
{
    [Header("Bounce Effect Parameters")]
    [SerializeField] private float initialDisplacement = .1f;
    [SerializeField] private float lerpSpeed = 1f;

    private Vector3 targetScale;

    void Start()
    {
        targetScale = transform.localScale;
    }

    void OnEnable()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - initialDisplacement, transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, lerpSpeed * Time.deltaTime);
    }
}
