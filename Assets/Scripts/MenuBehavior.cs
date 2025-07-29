using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBehavior : MonoBehaviour
{
    public GameObject dialogueBox;
    public Vector2 targetPos = new Vector2(900, -300);
    private bool hide = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            hide = true;
            Invoke("NextScene", 1);
        }

        if (hide)
        {
            dialogueBox.transform.position = Vector3.Lerp(dialogueBox.transform.position, targetPos, 5 * Time.deltaTime);
        }
    }
    void NextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIndex + 1);
    }
}
