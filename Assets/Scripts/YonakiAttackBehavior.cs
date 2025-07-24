using UnityEngine;

public class YonakiAttackBehavior : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float speed = 5f;
    private Transform target;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, Time.deltaTime * rotationSpeed);

        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;
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

        Invoke("Destroy", 2); // so the rocks don't fly away forever 
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}