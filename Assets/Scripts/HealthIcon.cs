// using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class HealthIcon : MonoBehaviour
{
    public Sprite max;
    public Sprite med;
    public Sprite low;
    private UnityEngine.UI.Image image;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = GetComponent<UnityEngine.UI.Image>();

    }

    // Update is called once per frame
    void Update()
    {
        int health = FindAnyObjectByType<PlayerHealth>().GetHealth();
        if (health == 3)
        {
            image.sprite = max;
        }
        else if (health == 2)
        {
            image.sprite = med;
        }
        else
        {
            image.sprite = low;
        }
    }
}
