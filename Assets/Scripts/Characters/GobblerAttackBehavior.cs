using UnityEngine;

public class GobblerAttackBehavior : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float delay;
    private Vector2 direction;
    private bool startMoving = false;

    // Update is called once per frame
    void Update()
    {
        if (direction == null)
        {
            Destroy(gameObject);
        }

        if (startMoving) transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    void StartMoving()
    {
        startMoving = true;
    }

    void Start()
    {
        transform.position = transform.position - (new Vector3(0, .5f, 0));
        startMoving = false;
        Invoke("StartMoving", delay);
        direction = new Vector2(1, 0);

        Invoke("Destroy", 4);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
