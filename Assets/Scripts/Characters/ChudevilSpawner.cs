using UnityEngine;

public class ChudevilSpawner : MonoBehaviour
{
    public GameObject chudevil;
    public static bool chudevilDead = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnReset.AddListener(Reset);
        }
    }

    // Update is called once per frame
    void Reset()
    {
        Instantiate(chudevil, transform.position, transform.rotation);
    }
}
