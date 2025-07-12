using UnityEngine;

public class IndicatorBehavior : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float distanceFromPlayer = 2f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float bounceSpeed = 1f;
    [SerializeField] private float bounceAmplitude = 1f;
    [SerializeField] private float fadeSpeed = 1f;
    private bool isVisible;

    void Start()
    {
        isVisible = false;
        ToggleVisible();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ToggleVisible();
        }

        // Direction and angle 
        Vector2 moveDir = player.GetComponent<PlayerMovement>().GetLastInput();

        var sp = GetComponent<SpriteRenderer>();
        if (PlayerMovement.Moving)
        {
            Color currentColor = sp.color;
            float targetAlpha = Mathf.Lerp(currentColor.a, 1f, Time.deltaTime * fadeSpeed);
            sp.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);

            float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg; // finds the angle the player is moving
            transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
        else
        {
            // Fade away
            Color currentColor = sp.color;
            float targetAlpha = Mathf.Lerp(currentColor.a, 0.025f, Time.deltaTime * fadeSpeed);
            sp.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);
        }

        Vector3 targetOffset = (Vector3)moveDir.normalized * distanceFromPlayer;
        Vector3 offset = targetOffset + targetOffset * Mathf.Sin(Time.time * bounceSpeed) * bounceAmplitude;
        transform.position = Vector3.Lerp(transform.position, player.transform.position + offset, speed * Time.deltaTime);
    }

    void ToggleVisible()
    {
        if (isVisible)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            isVisible = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = true;
            isVisible = true;
        }
    }
}
