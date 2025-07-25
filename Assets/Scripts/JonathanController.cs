using UnityEngine;

public class JonathanController : MonoBehaviour
{
    [Header("Child References")]
    public GameObject jondummy;       // Base body sprite
    public GameObject jonspeak;      // Talking animation
    public GameObject jondash_right; // Right dash animation
    public GameObject jonstrike_bolt_v2_0; // Lightning bolt effect
    public GameObject jonstrike_body_v2_0; // Body lightning effect

    [Header("Attack Settings")]
    public Transform target;
    public float acceleration = 5f;
    public float maxSpeed = 15f;
    public float overshootDeceleration = 8f;
    public float minOvershootDistance = 0.5f;
    public float maxOvershootDistance = 2f;
    public float strikeDuration = 1f;
    public float spinChargeDuration = 0.5f;
    public float spinDashSpeedMultiplier = 1.5f;
    public float spinSpeed = 720f;
    public float lightningDuration = 1.5f;

    [Header("Animation Parameters")]
    public Animator animator;
    public string attackParam = "attack";
    public string talkParam = "talk";

    // Private variables
    private Vector3 approachStart;
    private Vector3 targetPoint;
    private Vector3 overshootPoint;
    private Vector3 overshootDirection;
    private float currentSpeed = 0f;
    private float actualOvershoot;
    private float attackTimer;
    private float lightningTimer;
    private bool isApproaching = false;
    private bool isOvershooting = false;
    private bool isStriking = false;
    private bool isSpinCharging = false;
    private Quaternion initialBodyRotation;
    private int spinDirection = 1;

    void Start()
    {
        initialBodyRotation = jondummy.transform.localRotation;
        ResetAllAnimations();
        jondummy.SetActive(true);
    }

    void Update()
    {
        if (target == null) return;

        if (animator.GetBool(talkParam))
        {
            HandleTalking();
            return;
        }

        if (animator.GetBool(attackParam))
        {
            HandleAttackSequence();
        }
        else
        {
            ResetToIdle();
        }
    }

    void HandleTalking()
    {
        ResetAllAnimations();
        jonspeak.SetActive(true);
        jondummy.SetActive(true);
    }

    void HandleAttackSequence()
    {
        if (isStriking)
        {
            HandleStrikePhase();
        }
        else if (isSpinCharging)
        {
            HandleSpinChargePhase();
        }
        else if (isApproaching)
        {
            HandleSpinDashApproach();
        }
        else if (isOvershooting)
        {
            HandleOvershoot();
        }
        else
        {
            InitializeAttack();
        }
    }

    void InitializeAttack()
    {
        ResetAllAnimations();
        jonstrike_bolt_v2_0.SetActive(true); // Start bolt lightning
        if (jonstrike_body_v2_0 != null) jonstrike_body_v2_0.SetActive(true); // Start body lightning
        jondummy.SetActive(true);
        
        lightningTimer = lightningDuration;
        isStriking = true;
        attackTimer = strikeDuration;
        
        spinDirection = (target.position.x > transform.position.x) ? -1 : 1;
    }

    void HandleStrikePhase()
    {
        // Handle bolt lightning timer
        if (lightningTimer > 0)
        {
            lightningTimer -= Time.deltaTime;
            if (lightningTimer <= 0)
            {
                jonstrike_bolt_v2_0.SetActive(false); // Bolt ends on timer
            }
        }

        attackTimer -= Time.deltaTime;
        
        if (attackTimer <= 0)
        {
            isStriking = false;
            isSpinCharging = true;
            attackTimer = spinChargeDuration;
            FaceTarget();
            
            // Keep body lightning active during spin charge
            if (jonstrike_body_v2_0 != null) jonstrike_body_v2_0.SetActive(true);
            
            jondash_right.SetActive(true);
        }
    }

    void HandleSpinChargePhase()
    {
        // Spin with body lightning active
        jondummy.transform.Rotate(Vector3.forward, spinSpeed * spinDirection * Time.deltaTime);
        currentSpeed = Mathf.Min(currentSpeed + (acceleration * 2) * Time.deltaTime, maxSpeed * 0.7f);
        
        attackTimer -= Time.deltaTime;
        
        if (attackTimer <= 0)
        {
            isSpinCharging = false;
            InitializeSpinDashApproach();
            jondummy.transform.localRotation = initialBodyRotation;
            
            // Turn off body lightning when dash starts
            if (jonstrike_body_v2_0 != null) jonstrike_body_v2_0.SetActive(false);
            jondash_right.SetActive(false);
        }
    }

    void InitializeSpinDashApproach()
    {
        jondummy.SetActive(true);
        
        approachStart = transform.position;
        targetPoint = target.position;
        isApproaching = true;
        currentSpeed *= spinDashSpeedMultiplier;
        
        overshootDirection = (targetPoint - approachStart).normalized;
        actualOvershoot = Mathf.Lerp(minOvershootDistance, maxOvershootDistance, 
                                   Mathf.InverseLerp(0, maxSpeed, currentSpeed));
        overshootPoint = targetPoint + overshootDirection * actualOvershoot;
    }

    void HandleSpinDashApproach()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, currentSpeed * Time.deltaTime);
        jondummy.transform.Rotate(Vector3.forward, spinSpeed * 0.5f * spinDirection * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint) < 0.01f)
        {
            isApproaching = false;
            isOvershooting = true;
        }
    }

    void HandleOvershoot()
    {
        currentSpeed = Mathf.Max(currentSpeed - overshootDeceleration * Time.deltaTime, 0f);
        transform.position = Vector3.MoveTowards(transform.position, overshootPoint, currentSpeed * Time.deltaTime);

        if (currentSpeed < 0.1f)
        {
            isOvershooting = false;
            if (animator.GetBool(attackParam))
            {
                InitializeAttack();
            }
        }
    }

    void ResetToIdle()
    {
        isApproaching = false;
        isOvershooting = false;
        isStriking = false;
        isSpinCharging = false;
        ResetAllAnimations();
        jondummy.SetActive(true);
        jondummy.transform.localRotation = initialBodyRotation;
    }

    void ResetAllAnimations()
    {
        jonspeak.SetActive(false);
        jondash_right.SetActive(false);
        jonstrike_bolt_v2_0.SetActive(false);
        if (jonstrike_body_v2_0 != null) jonstrike_body_v2_0.SetActive(false);
    }

    void FaceTarget()
    {
        transform.localScale = new Vector3(
            target.position.x > transform.position.x ? 1 : -1,
            1, 1);
    }
}