using UnityEngine;

public class YonakiAttackBehavior : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float speed = 5f;
    private Transform target;
    private Vector2 direction;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, Time.deltaTime * rotationSpeed);

        if (target == null) return;

        if (direction == null)
        {
            Destroy(gameObject);
        }

        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    void Start()
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

        // We need to do this after we find the player
        direction = (target.position - transform.position).normalized;

        Invoke("Destroy", 10); // so the rocks don't fly away forever 
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}