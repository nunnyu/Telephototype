using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class OnPicture : MonoBehaviour
{
    [Header("Parent object of the child using this script, for position purposes.")]
    [SerializeField] GameObject parent;
    [SerializeField] GameObject indicator;

    [Header("Bounce Effect Parameters")]
    [SerializeField] public float initialDisplacement = .1f;
    [SerializeField] private float lerpSpeed = 1f;

    [Header("Attack")]
    [SerializeField] private float distanceFromPlayer = 1f;
    [SerializeField] private float attackDelay = .66f;
    [SerializeField] private float attackLength = .1f;
    [SerializeField] private GameObject hitbox;
    [SerializeField] private GameObject camFlash;

    private Vector3 targetScale;
    private Vector3 targetPos;
    private bool parried;

    void Awake()
    {
        hitbox.SetActive(false);
        camFlash.SetActive(false);
        parried = false;
        targetScale = transform.localScale;
    }

    void OnEnable()
    {
        parried = false;
        // Bounce effect on enable 
        transform.localScale = new Vector3(targetScale.x, targetScale.y - initialDisplacement, transform.localScale.z);
        targetPos = new Vector3(parent.transform.position.x, parent.transform.position.y, transform.position.z);

        Invoke("Attack", attackDelay);
    }

    // Update is called once per frame
    void Update()
    {
        // Bounce animation 
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, lerpSpeed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
    }

    void Attack()
    {
        Vector3 direction = (indicator.transform.position - transform.position).normalized;
        Vector3 targetOffset = direction * distanceFromPlayer;
        hitbox.transform.position = targetOffset + transform.position;
        hitbox.transform.rotation = indicator.transform.rotation;
        hitbox.SetActive(true);
        camFlash.SetActive(true);

        Invoke("EndAttack", attackLength);
        // turn on the flash for a bit, and then 
    }

    void EndAttack()
    {
        hitbox.SetActive(false);
        camFlash.SetActive(false);
        // Turn off 
    }
}