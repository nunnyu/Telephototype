using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static event System.Action OnTutorialFightStart;
    public static event System.Action OnTutorialFightEnd;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void TriggerTutorialFightStart()
    {
        OnTutorialFightStart?.Invoke();
    }

    public void TriggerTutorialFightEnd()
    {
        OnTutorialFightEnd?.Invoke();
    }
}