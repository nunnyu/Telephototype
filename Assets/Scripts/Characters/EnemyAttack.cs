using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Bounce Effect Parameters")]
    [SerializeField] public float bounceScaleDown = .1f;
    [SerializeField] private float lerpSpeed = 1f;

    [Header("Attack & Auto-Attack Parameters")]
    [SerializeField] private bool isAutoAttack = true;
    [SerializeField] private Transform target;
    [SerializeField] private float distanceFromEnemy = 1f;
    [SerializeField] private float attackDelay = .66f;
    [SerializeField] private GameObject attackObj;
    [SerializeField] private float vulnerableLength = 1f;
    public bool IsAttacking { get; set; }
    private Vector3 targetScale;
    private Vector3 targetPos;

    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnReset.AddListener(Reset);
        }
    }

    void Reset()
    {
        target = null;
    }

    void Awake()
    {
        targetScale = transform.localScale;
        targetPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    public Transform GetTarget()
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

        return target;
    }

    public void Attack()
    {
        GetTarget();

        // Bounce Animation
        transform.localScale = new Vector3(targetScale.x, targetScale.y - bounceScaleDown, transform.localScale.z);
        transform.position = new Vector3(transform.position.x, transform.position.y - bounceScaleDown / 2f, transform.position.z);

        Invoke("ActivateInstance", attackDelay);
    }

    void Update()
    {
        if (isAutoAttack)
        {
            // Bounce animation 
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, lerpSpeed * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
        }
    }

    void ActivateInstance()
    {
        IsAttacking = true;
        Vector3 direction = new Vector3(0, 0, 0);
        
        if (target != null && transform != null)
        {
            direction = (target.position - transform.position).normalized;
        }
        
        Vector3 targetOffset = direction * distanceFromEnemy;
        var targetPos = targetOffset + transform.position;

        Instantiate(attackObj, targetPos, Quaternion.identity);
        // Rotation will be handled in the prefab

        Invoke("EndVulnerability", vulnerableLength);
    }

    void EndVulnerability()
    {
        IsAttacking = false;
    }
}
