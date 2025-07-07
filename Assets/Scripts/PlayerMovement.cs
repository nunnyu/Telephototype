using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 5f;
    public static bool Moving { get; private set; }
    private Vector2 input;
    private float x;
    private float y;

    // Inputs
    void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        input = Vector2.right * x + Vector2.up * y;
        input.Normalize();

        if (input.magnitude != 0)
        {
            Moving = true;
        }
        else
        {
            Moving = false;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + input * moveSpeed * Time.fixedDeltaTime);
    }
}