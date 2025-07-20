using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Bounce Effect Parameters")]
    [SerializeField] public float bounceScaleDown = .1f;
    // [SerializeField] public float bounceInitialPos = .1f;
    [SerializeField] private float lerpSpeed = 1f;

    [Header("Attack")]
    [SerializeField] private Transform target;
    [SerializeField] private float distanceFromEnemy = 1f;
    [SerializeField] private float attackDelay = .66f;
    [SerializeField] private GameObject attackObj;

    private Vector3 targetScale;
    private Vector3 targetPos;

    void Awake()
    {
        targetScale = transform.localScale;
        targetPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    public void Attack()
    {
        var targetObj = GameObject.FindGameObjectWithTag("Player");

        if (targetObj && targetObj.activeInHierarchy)
        {
            target = targetObj.transform;
        }
        else
        {
            target = GameObject.FindGameObjectWithTag("CameraPoses").transform;
        }

        // Bounce Animation
            transform.localScale = new Vector3(targetScale.x, targetScale.y - bounceScaleDown, transform.localScale.z);
        transform.position = new Vector3(transform.position.x, transform.position.y - bounceScaleDown / 2f, transform.position.z);

        Invoke("ActivateInstance", attackDelay);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Attack();
        }

        // Bounce animation 
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, lerpSpeed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
    }

    void ActivateInstance()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Vector3 targetOffset = direction * distanceFromEnemy;
        var targetPos = targetOffset + transform.position;

        Instantiate(attackObj, targetPos, Quaternion.identity);
        // Rotation will be handled in the prefab
    }
}
