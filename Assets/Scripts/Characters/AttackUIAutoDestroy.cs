using UnityEngine;

public class AttackUIAutoDestroy : MonoBehaviour
{
    public GameObject other;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!other.activeInHierarchy)
        {
            Destroy(gameObject);
        }
    }
}
