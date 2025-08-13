using UnityEngine;

public class Autodestroy : MonoBehaviour
{
    public int delay = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("SelfDestruct", 2);
    }

    void SelfDestruct()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
