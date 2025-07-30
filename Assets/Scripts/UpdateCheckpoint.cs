using UnityEngine;

public class UpdateCheckpoint : MonoBehaviour
{
    public int checkpointNumber = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("New checkpoint unlocked.");
            if (GameManager.Instance != null)
            {
                other.GetComponent<PlayerHealth>().ResetHealth();
                GameManager.Instance.rinkoSpawn = transform.position;
                GameManager.Instance.UpdateCheckpoint(checkpointNumber);
            }
        }
    }
}
