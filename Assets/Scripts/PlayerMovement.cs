using System;
using System.Collections;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Serialized Editor Fields 
    [Header("Movement / Physics")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 5f;

    [Header("For walking backwards")]
    [SerializeField] private Vector3 scarfBitDisjoint;

    [Header("Collisions")]
    [SerializeField] private Vector2 raycastBoxSize = new Vector2(1, 1);
    [SerializeField] private LayerMask collisionLayerMask;
    [SerializeField] private float offset = -.5f;

    // Movement Delay
    [Header("Movement Delay")]
    [SerializeField] private float postPictureStun = .2f;

    // GameObjects for Animations 
    [Header("Body Objects: these should be sprites, not the parent objects.")]
    [SerializeField] private GameObject scarf;
    [SerializeField] private GameObject scarfBit;
    [SerializeField] private GameObject leftShoulder;
    [SerializeField] private GameObject rightShoulder;
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;

    // Positions for walking right
    [Header("Limb Positions: walking right.")]
    [SerializeField] private Vector3 scarfSide;
    [SerializeField] private Vector3 scarfSideScale;
    [SerializeField] private Vector3 scarfBitSide;
    [SerializeField] private Vector3 leftShoulderSide;
    [SerializeField] private Vector3 rightShoulderSide;
    [SerializeField] private Vector3 leftHandSide;
    [SerializeField] private Vector3 rightHandSide;
    [SerializeField] private int scarfBitSortingOrder;
    [SerializeField] private int leftShoulderSortingOrder;
    [SerializeField] private int rightShoulderSortingOrder;
    [SerializeField] private int leftHandSortingOrder;
    [SerializeField] private int rightHandSortingOrder;

    // Rotations for walking right 
    [SerializeField] private Quaternion LeftShoulderRotation;
    [SerializeField] private Quaternion RightShoulderRotation;
    [SerializeField] private Quaternion LeftHandRotation;
    [SerializeField] private Quaternion RightHandRotation;

    // Properties
    public static bool Moving { get; private set; }
    public static bool MovingUp { get; private set; }
    public static bool MovingDown { get; private set; }
    public static bool MovingLeft { get; private set; }
    public static bool MovingRight { get; private set; }

    // Inputs
    private Vector2 input; // (x, y) is the direction
    private float x; // horizontal input direction
    private float y; // vertical input direction 

    // Walk Forward Default Position
    private Vector3 scarfDefaultPos;
    private Vector3 scarfBitDefaultPos;
    private Vector3 leftShoulderDefaultPos;
    private Vector3 rightShoulderDefaultPos;
    private Vector3 leftHandDefaultPos;
    private Vector3 rightHandDefaultPos;

    // Sorting Orders
    private int defaultScarfBitSortingOrder;
    private int defaultLeftShoulderSortingOrder;
    private int defaultRightShoulderSortingOrder;
    private int defaultLeftHandSortingOrder;
    private int defaultRightHandSortingOrder;

    // Default Rotations
    private Quaternion defaultLeftShoulderRotation;
    private Quaternion defaultRightShoulderRotation;
    private Quaternion defaultLeftHandRotation;
    private Quaternion defaultRightHandRotation;

    // Flags
    public static bool CanMove;
    public static bool DialogueLock;
    private Vector2 lastNonZeroInput;

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other);
    }

    IEnumerator DelayedAction(System.Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

    void OnEnable()
    {
        CanMove = false;
        StartCoroutine(DelayedAction(() =>
        {
            CanMove = true;
        }, postPictureStun));
    }

    // Initialize everything
    void Start()
    {
        // GetComponent<Rigidbody>().freezeRotation = true;

        // Let's just say that when you start idle, you have "input" down
        lastNonZeroInput = new Vector2(0, -1f);

        // Calculate local offsets from the player's transform
        scarfDefaultPos = scarf.transform.position - transform.position;
        scarfBitDefaultPos = scarfBit.transform.position - transform.position;
        leftShoulderDefaultPos = leftShoulder.transform.position - transform.position;
        rightShoulderDefaultPos = rightShoulder.transform.position - transform.position;
        leftHandDefaultPos = leftHand.transform.position - transform.position;
        rightHandDefaultPos = rightHand.transform.position - transform.position;

        defaultLeftShoulderSortingOrder = leftShoulder.GetComponent<Renderer>().sortingOrder;
        defaultRightShoulderSortingOrder = rightShoulder.GetComponent<Renderer>().sortingOrder;
        defaultLeftHandSortingOrder = leftHand.GetComponent<Renderer>().sortingOrder;
        defaultRightHandSortingOrder = leftHand.GetComponent<Renderer>().sortingOrder;
        defaultScarfBitSortingOrder = scarfBit.GetComponent<Renderer>().sortingOrder;

        defaultLeftShoulderRotation = leftShoulder.transform.rotation;
        defaultRightShoulderRotation = rightShoulder.transform.rotation;

        CanMove = true;
    }

    // Inputs
    void Update()
    {
        // If Rinko can't move, for however long, she will just idle animation at the camera
        if (CanMove && !DialogueLock)
        {
            ProcessInputs();
            UpdateMovementProperties();
            HandleMovementAnimations();
            HandleArmMovements();
            PositionScarf();
        }
        else
        {
            Moving = false;
        }

        if (DialogueLock)
        {
            // Rinko should be idle while listening / performing a dialogue 
            GetComponent<Animator>().SetBool("Moving", false);
            GetComponent<Animator>().SetBool("Walk_Side", false);
            GetComponent<Animator>().SetBool("Walk_Up", false);
            GetComponent<Animator>().SetBool("Walk_Down", false);
            ShiftLimbs(false, false);
        }

        if (input.magnitude != 0)
        {
            lastNonZeroInput = input;
        }
    }

    void FixedUpdate()
    {
        Vector2 direction = input.normalized;
        float distance = moveSpeed * Time.fixedDeltaTime;
        Vector2 position = new Vector3(transform.position.x, transform.position.y + offset);

        RaycastHit2D[] hits = Physics2D.BoxCastAll(position, raycastBoxSize, 0f, direction, distance, collisionLayerMask);

        bool blocked = false;
        float shortestSafeDistance = distance;

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && !hit.collider.isTrigger)
            {
                blocked = true;
                shortestSafeDistance = Mathf.Min(shortestSafeDistance, hit.distance - 0.01f); // Tiny buffer
            }
        }

        if (CanMove && !DialogueLock)
        {
            if (blocked)
            {
                rb.MovePosition(rb.position + direction * shortestSafeDistance);
            }
            else
            {
                Move(); // Nothing is blocking our position
            }
        }
    }

    void Move()
    {
        rb.MovePosition(rb.position + input * moveSpeed * Time.fixedDeltaTime);
    }

    void OnDrawGizmos()
    {
        // For collision detection 
        Vector3 positionForGizmo = new Vector3(transform.position.x, transform.position.y + offset);
        Gizmos.DrawWireCube(positionForGizmo, raycastBoxSize);
    }

    void ProcessInputs()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        if (!CanMove || DialogueLock)
        {
            x = 0;
            y = 0;
        }

        input = Vector2.right * x + Vector2.up * y;
        input.Normalize();
    }

    public void ResetLimbs()
    {
        // Reset positions (relative to player)
        scarf.transform.position = transform.position + scarfDefaultPos;
        scarfBit.transform.position = transform.position + scarfBitDefaultPos;
        leftShoulder.transform.position = transform.position + leftShoulderDefaultPos;
        rightShoulder.transform.position = transform.position + rightShoulderDefaultPos;
        leftHand.transform.position = transform.position + leftHandDefaultPos;
        rightHand.transform.position = transform.position + rightHandDefaultPos;

        // Reset sorting orders
        scarfBit.GetComponent<Renderer>().sortingOrder = defaultScarfBitSortingOrder;
        leftShoulder.GetComponent<Renderer>().sortingOrder = defaultLeftShoulderSortingOrder;
        rightShoulder.GetComponent<Renderer>().sortingOrder = defaultRightShoulderSortingOrder;
        leftHand.GetComponent<Renderer>().sortingOrder = defaultLeftHandSortingOrder;
        rightHand.GetComponent<Renderer>().sortingOrder = defaultRightHandSortingOrder;

        // Reset rotations
        leftShoulder.transform.rotation = defaultLeftShoulderRotation;
        rightShoulder.transform.rotation = defaultRightShoulderRotation;
        leftHand.transform.rotation = defaultLeftHandRotation;
        rightHand.transform.rotation = defaultRightHandRotation;
    }

    // For moving sideways 
    void ShiftLimbs(bool walkingSideways, bool walkingRight)
    {
        if (!walkingSideways)
        {
            ResetLimbs();
        }
        else
        {
            // Shift to side positions (also relative to player)
            scarf.transform.position = transform.position + scarfSide;
            scarfBit.transform.position = transform.position + scarfBitSide;
            leftShoulder.transform.position = transform.position + leftShoulderSide;
            rightShoulder.transform.position = transform.position + rightShoulderSide;
            leftHand.transform.position = transform.position + leftHandSide;
            rightHand.transform.position = transform.position + rightHandSide;

            int flip;
            if (walkingRight)
            {
                flip = 1;
                scarf.transform.localScale = scarfSideScale;
            }
            else
            {
                flip = -1;
                scarf.transform.localScale = Vector3.Scale(scarfSideScale, new Vector3(-1, 1, 1));
            }

            scarf.transform.position = transform.position + new Vector3(scarfSide.x * flip, scarfSide.y, scarfSide.z);
            scarfBit.transform.position = transform.position + new Vector3(scarfBitSide.x * flip, scarfBitSide.y, scarfBitSide.z);
            leftShoulder.transform.position = transform.position + new Vector3(leftShoulderSide.x * flip, leftShoulderSide.y, leftShoulderSide.z);
            rightShoulder.transform.position = transform.position + new Vector3(rightShoulderSide.x * flip, rightShoulderSide.y, rightShoulderSide.z);
            leftHand.transform.position = transform.position + new Vector3(leftHandSide.x * flip, leftHandSide.y, leftHandSide.z);
            rightHand.transform.position = transform.position + new Vector3(rightHandSide.x * flip, rightHandSide.y, rightHandSide.z);

            // Update sorting orders for side view
            scarfBit.GetComponent<Renderer>().sortingOrder = scarfBitSortingOrder;
            leftShoulder.GetComponent<Renderer>().sortingOrder = leftShoulderSortingOrder;
            rightShoulder.GetComponent<Renderer>().sortingOrder = rightShoulderSortingOrder;
            leftHand.GetComponent<Renderer>().sortingOrder = leftHandSortingOrder;
            rightHand.GetComponent<Renderer>().sortingOrder = rightHandSortingOrder;

            // Update rotation
            if (walkingRight)
            {
                leftShoulder.transform.rotation = LeftShoulderRotation;
                rightShoulder.transform.rotation = RightShoulderRotation;
                leftHand.transform.rotation = LeftHandRotation;
                rightHand.transform.rotation = RightHandRotation;
            }
            else
            {
                leftShoulder.transform.rotation = new Quaternion(LeftShoulderRotation.x, LeftShoulderRotation.y, LeftShoulderRotation.z * -1, LeftShoulderRotation.w);
                rightShoulder.transform.rotation = new Quaternion(RightShoulderRotation.x, RightShoulderRotation.y, RightShoulderRotation.z * -1, RightShoulderRotation.w);
                leftHand.transform.rotation = new Quaternion(LeftHandRotation.x, LeftHandRotation.y, LeftHandRotation.z * -1, LeftHandRotation.w);
                rightHand.transform.rotation = new Quaternion(RightHandRotation.x, RightHandRotation.y, RightHandRotation.z * -1, RightHandRotation.w);
                scarfBit.GetComponent<Renderer>().sortingOrder = scarf.GetComponent<Renderer>().sortingOrder - 1;
            }
        }
    }

    void UpdateMovementProperties()
    {
        if (input.magnitude != 0)
        {
            Moving = true;
        }
        else
        {
            Moving = false;
        }

        if (x > 0.05)
        {
            MovingRight = true;
        }
        else
        {
            MovingRight = false;
        }

        if (x < -0.05)
        {
            MovingLeft = true;
        }
        else
        {
            MovingLeft = false;
        }

        if (y > 0.05)
        {
            MovingUp = true;
        }
        else
        {
            MovingUp = false;
        }

        if (y < -0.05)
        {
            MovingDown = true;
        }
        else
        {
            MovingDown = false;
        }
    }

    void HandleMovementAnimations()
    {
        // Default direction to face
        transform.localScale = new Vector3(1, 1, 1);

        // Handle Animations
        var animator = GetComponent<Animator>();

        if (Moving)
        {
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }

        if (MovingDown)
        {
            animator.SetBool("Walk_Down", true);
        }
        else
        {
            animator.SetBool("Walk_Down", false);
        }

        if (MovingUp)
        {
            animator.SetBool("Walk_Up", true);
        }
        else
        {
            animator.SetBool("Walk_Up", false);
        }

        if ((MovingRight || MovingLeft) && !(MovingUp || MovingDown))
        {
            if (MovingLeft)
            {
                transform.localScale = new Vector3(-1, 1, 1); // Flip the model, facing right is default
                ShiftLimbs(true, false);
            }
            else
            {
                ShiftLimbs(true, true);
            }

            animator.SetBool("Walk_Side", true);
        }
        else
        {
            animator.SetBool("Walk_Side", false);
        }

        if (!animator.GetBool("Walk_Side"))
        {
            ShiftLimbs(false, false);
        }
    }

    void PositionScarf()
    {
        if (Moving)
        {
            if (MovingUp)
            {
                scarf.transform.localScale = new Vector3(-1, 1, 1);
                scarfBit.transform.position = transform.position - scarfBitDisjoint;
            }
            if (MovingDown)
            {
                scarf.transform.localScale = new Vector3(1, 1, 1);
                scarfBit.transform.position = transform.position + scarfBitDisjoint;
            }
        }
        else
        {
            scarf.transform.localScale = new Vector3(1, 1, 1);
            scarfBit.transform.position = transform.position + scarfBitDisjoint;
        }
    }

    void HandleArmMovements()
    {
        // When she walks up, her back is showing, and her arms are in front of her body
        int walkDownOrder = 12;
        int walkUpOrder = 6;

        if (Moving)
        {
            if (MovingDown)
            {
                leftHand.GetComponent<Renderer>().sortingOrder = walkDownOrder;
                rightHand.GetComponent<Renderer>().sortingOrder = walkDownOrder;
            }

            if (MovingUp)
            {
                leftHand.GetComponent<Renderer>().sortingOrder = walkUpOrder;
                rightHand.GetComponent<Renderer>().sortingOrder = walkUpOrder;
            }
        }
        else
        {
            // During idle it's the same as if you are walking down, so it's in front of the body
            leftHand.GetComponent<Renderer>().sortingOrder = walkDownOrder;
            rightHand.GetComponent<Renderer>().sortingOrder = walkDownOrder;
        }
    }

    public Vector2 GetLastInput()
    {
        return lastNonZeroInput;
    }
}