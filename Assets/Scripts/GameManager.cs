using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static int spiritsCaptured = 0;

    public Vector2 rinkoSpawn = new Vector2(0, -2);
    public UnityEvent OnTutorialFightStart;
    public UnityEvent OnTutorialFightEnd;
    public UnityEvent GobblersDefeated;
    public UnityEvent OnReset; // for when the player dies, or they go back a save point
    public bool IsTutorialFightActive { get; private set; } = false;
    public GameObject fadeIn;
    private int checkpoint = 1;

    public void UpdateCheckpoint(int checkpoint)
    {
        this.checkpoint = checkpoint;
    }

    void Update()
    {
        Debug.Log("Tutorial Fight Active? " + IsTutorialFightActive); // just for testing

        if (GobblerAnimationHandler.gobblerCount == 3)
        {
            GobblersDefeated?.Invoke();
        }
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

    public void PartOneEnd()
    {
        Instantiate(fadeIn);
        Invoke("LoadNextScene", 3);
    }

    public void LoadNextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIndex + 1);
    }

    public void TriggerReset()
    {
        Debug.Log("RESETTING WORLD");
        OnReset?.Invoke();
    }

    void ResetSpirits()
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // These extra checks are to ensure we are JUST checking this scene, "type all" object search is very powerful
            if (obj.CompareTag("Spirit") &&
                obj.hideFlags == HideFlags.None &&
                obj.scene.IsValid() &&
                obj.scene == SceneManager.GetActiveScene())
            {
                if (checkpoint == 1)
                {
                    obj.SetActive(true);
                    spiritsCaptured = 1; // not zero because Haneko is always captured before the first death 
                }
                else if (checkpoint == 2)
                {
                    spiritsCaptured = 5;
                    // do nothing(?) 
                }
            }
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