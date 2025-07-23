using UnityEngine;
using UnityEngine.Events;

public class ProgressionBlocker : MonoBehaviour
{
    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnTutorialFightEnd.AddListener(RemoveBlocker);
        }
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnTutorialFightEnd.RemoveListener(RemoveBlocker);
        }
    }

    void RemoveBlocker()
    {
        Debug.Log("Removing blocker");
        Destroy(gameObject);
    }
}
