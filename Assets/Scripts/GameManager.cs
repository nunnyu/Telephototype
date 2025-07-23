using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public UnityEvent OnTutorialFightStart;
    public UnityEvent OnTutorialFightEnd;

    public bool IsTutorialFightActive { get; private set; } = false;

    void Update()
    {
        Debug.Log("Tutorial Fight Active? " + IsTutorialFightActive); // just for testing
    }

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
        Debug.Log("Fight starts.");
        IsTutorialFightActive = true;
        OnTutorialFightStart?.Invoke();
    }

    public void TriggerTutorialFightEnd()
    {
        IsTutorialFightActive = false;
        OnTutorialFightEnd?.Invoke();
    }
}