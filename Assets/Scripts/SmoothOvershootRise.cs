using UnityEngine;

public class SmoothOvershootRise : MonoBehaviour
{
    [Header("Left Sprite")]
    public GameObject leftAnim;

    [Header("Idle Sprite")]
    public GameObject idleChud;

    [Header("Right Sprite")]
    public GameObject rightAnim;

    [Header("Target Settings")]
    public Transform target;
    private Vector3 approachStart;
    private Vector3 targetPoint;
    private bool isApproaching = false;
    public bool isOvershooting = false;
    public bool isRising = false;

    [Header("Movement Settings")]
    public float acceleration = 5f;
    public float maxSpeed = 10f;
    public float overshootDeceleration = 8f;
    private float currentSpeed = 0f;

    [Header("Overshoot Settings")]
    public float minOvershootDistance = 0.5f;
    public float maxOvershootDistance = 2f;
    public float actualOvershoot;
    public Vector3 overshootPoint;
    public Vector3 overshootDirection;

    [Header("Rise Settings")]
    public float riseHeight = 1f;
    public float riseDuration = 0.5f;
    private Vector3 riseStart;
    private float riseProgress;
    float targetX;
    float objectX;

    [Header("idle/attack animatore")]
    public Animator childAnimator;
    public string alartparamatar = "alert";

    private Vector2 startPos;

    void Start()
    {
        startPos = transform.position;
        riseStart = transform.position;
        if (target) targetPoint = target.position;
    }

    void Update()
    {
        if (childAnimator.GetBool(alartparamatar)) return;
        target = GetComponent<EnemyAttack>().GetTarget();
        if (target == null) return;

        if (GetComponent<EnemyAttack>().IsAttacking)
        {
            if (!isApproaching && !isOvershooting && !isRising)
            {
                InitializeApproach();
            }

            if (isApproaching)
            {
                HandleApproach();
            }
            else if (isOvershooting)
            {
                HandleOvershoot();
            }
            else if (isRising)
            {
                HandleRise();
            }
        }
    }

    void InitializeApproach()
    {
        targetX = target.position.x;
        objectX = transform.position.x;
        if (objectX > targetX)
        {
            leftAnim.SetActive(true);
            idleChud.SetActive(false);
            rightAnim.SetActive(false);
        }
        else
        {
            leftAnim.SetActive(false);
            idleChud.SetActive(false);
            rightAnim.SetActive(true);
        }
        approachStart = transform.position;
        targetPoint = target.position;
        isApproaching = true;
        currentSpeed = 0f;

        // Calculate dynamic overshoot based on approach speed
        float speedRatio = Mathf.InverseLerp(0, maxSpeed, currentSpeed);
        actualOvershoot = Mathf.Lerp(minOvershootDistance, maxOvershootDistance, speedRatio);
    }

    void HandleApproach()
    {
        // Accelerate toward target
        currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxSpeed);
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, currentSpeed * Time.deltaTime);

        // When target is reached, setup overshoot
        if (Vector3.Distance(transform.position, targetPoint) < 0.01f)
        {
            overshootDirection = (targetPoint - approachStart).normalized;
            overshootPoint = targetPoint + overshootDirection * actualOvershoot;
            isApproaching = false;
            isOvershooting = true;
        }
    }

    void HandleOvershoot()
    {
        // Decelerate while moving toward overshoot point
        currentSpeed = Mathf.Max(currentSpeed - overshootDeceleration * Time.deltaTime, 0f);
        transform.position = Vector3.MoveTowards(transform.position, overshootPoint, currentSpeed * Time.deltaTime);

        // When nearly stopped or reached overshoot point, start rising
        if (currentSpeed < 0.1f || Vector3.Distance(transform.position, overshootPoint) < 0.01f)
        {
            riseStart = transform.position;
            riseProgress = 0f;
            isOvershooting = false;
            isRising = true;
        }
    }

    void HandleRise()
    {
        leftAnim.SetActive(false);
        idleChud.SetActive(true);
        rightAnim.SetActive(false);

        // Smooth rise with easing
        riseProgress = Mathf.Min(riseProgress + Time.deltaTime / riseDuration, 1f);
        float easedProgress = EaseOutQuad(riseProgress);
        transform.position = riseStart + Vector3.up * (easedProgress * riseHeight);

        if (riseProgress >= 1f)
        {
            isRising = false;
        }
    }

    float EaseOutQuad(float t)
    {
        return 1 - (1 - t) * (1 - t);
    }

    void OnDrawGizmos()
    {
        if (target == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(targetPoint, 0.2f);

        if (isApproaching)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(approachStart, targetPoint);
        }

        if (isOvershooting)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(targetPoint, overshootPoint);
            Gizmos.DrawWireSphere(overshootPoint, 0.15f);
        }

        if (isRising)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(riseStart, riseStart + Vector3.up * riseHeight);
        }
    }
}