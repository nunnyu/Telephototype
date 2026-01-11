using UnityEngine;

public class HitboxBehavior : MonoBehaviour
{
    public GameObject spawnablePrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }

    void OnEnable()
    {
        Instantiate(spawnablePrefab);
    }

    // Update is called once per frame
    void Update()
    {
        if (ChudevilSpawner.chudevilDead == true)
        {
            Invoke("UpdateSize", 1);
        }
    }

    void UpdateSize()
    {
        transform.localScale = new Vector3(.6f, 2.5f, 1);
    }
}
