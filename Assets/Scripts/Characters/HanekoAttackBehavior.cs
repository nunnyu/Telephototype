using UnityEngine;

public class HanekoAttackBehavior : MonoBehaviour
{
    [SerializeField] private float destroyDelay = .5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("SelfDestruct", destroyDelay);
    }

    void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
