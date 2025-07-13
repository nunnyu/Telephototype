using UnityEngine;
using UnityEngine.Animations;

public class OnPicture : MonoBehaviour
{
    [Header("Parent object of the child using this script, for position purposes.")]
    [SerializeField] GameObject parent;

    [Header("Bounce Effect Parameters")]
    [SerializeField] public float initialDisplacement = .1f;
    [SerializeField] private float lerpSpeed = 1f;
    private Vector3 targetScale;
    private Vector3 targetPos;

    void Awake()
    {
        targetScale = transform.localScale;
    }

    void OnEnable()
    {
        transform.localScale = new Vector3(targetScale.x, targetScale.y - initialDisplacement, transform.localScale.z);
        targetPos = new Vector3(parent.transform.position.x, parent.transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(transform.position);
        // Debug.Log(targetPos);
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, lerpSpeed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
    }
}
