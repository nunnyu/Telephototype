using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Vector2 rinkoSpawn = new Vector2(0, -2);
    public UnityEvent OnTutorialFightStart;
    public UnityEvent OnTutorialFightEnd;
    public UnityEvent OnReset; // for when the player dies, or they go back a save point
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

        OnReset.AddListener(ResetSpirits);
    }

    public void TriggerReset()
    {
        Debug.Log("RESETTING WORLD");
        OnReset?.Invoke();
    }

    void ResetSpirits()
    {
        var spirits = GameObject.FindGameObjectsWithTag("Spirit");
        foreach (var spirit in spirits)
        {
            spirit.SetActive(true);
        }
    }

    // This is referenced by a trigger event zone object
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