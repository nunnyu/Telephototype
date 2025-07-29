using UnityEngine;

public class HungernAttackBehavior : MonoBehaviour
{
    public float offset = .5f;
    public float posSpeed = 1f;
    public float scaleSpeed = 1f;
    public float destroyDelay = 1f;
    public float targetScale = 1.4f;
    public float switchDirectionsTime = .5f;
    private Vector2 targetPos;
    private Vector2 originalPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalPos = transform.position;
        Invoke("DestroySelf", destroyDelay);
        targetPos = new Vector2(transform.position.x, transform.position.y + offset);
        Invoke("SwitchDirections", switchDirectionsTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(targetScale, targetScale, targetScale), scaleSpeed * Time.deltaTime);
        transform.position = Vector2.MoveTowards(transform.position, targetPos, posSpeed * Time.deltaTime);
    }

    void SwitchDirections()
    {
        targetPos = originalPos;
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
