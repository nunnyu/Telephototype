using UnityEngine;

public class RespawnSound : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnReset.AddListener(PlaySound);
        }
    }

    void PlaySound()
    {
        GetComponent<AudioSource>().Play();
    }
}
