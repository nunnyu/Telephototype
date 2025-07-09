using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Serialized Editor Fields 
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Vector3 scarfBitDisjoint;

    // GameObjects for Animations 
    [Header("Body Objects: these should be sprites, not the parent objects.")]
    [SerializeField] private GameObject scarf;
    [SerializeField] private GameObject scarfBit;
    [SerializeField] private GameObject leftShoulder;
    [SerializeField] private GameObject rightShoulder;
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;

    // Properties
    public static bool Moving { get; private set; }
    public static bool MovingUp { get; private set; }
    public static bool MovingDown { get; private set; }
    public static bool MovingLeft { get; private set; }
    public static bool MovingRight { get; private set; }

    // Private or Flags 
    private Vector2 input;
    private float x;
    private float y;
    private Vector2 scarfBitDefaultPos;

    // Initialize everything
    void Start()
    {
        scarfBitDefaultPos = scarfBit.transform.position;
    }

    // Inputs
    void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        input = Vector2.right * x + Vector2.up * y;
        input.Normalize();

        UpdateMovementProperties();
        HandleMovementAnimations();
        HandleArmMovements();
        PositionScarf();
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
        // Handle Animations
        var animator = GetComponent<Animator>();

        // Default direction to face
        transform.localScale = new Vector3(1, 1, 1);

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

        if (MovingRight || MovingLeft)
        {
            if (MovingLeft)
            {
                transform.localScale = new Vector3(-1, 1, 1); // Flip the model, facing right is default
            }

            animator.SetBool("Walk_Side", true);
        }
        else
        {
            animator.SetBool("Walk_Side", false);
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

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + input * moveSpeed * Time.fixedDeltaTime);
    }
}