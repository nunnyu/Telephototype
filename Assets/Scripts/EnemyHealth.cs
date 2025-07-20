using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 3;
    [SerializeField] private float colorLerpSpeed = 5f;
    [SerializeField] private SpriteRenderer sr;
    private Color originalColor;
    private List<Transform> children;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalColor = sr.color;

        children = new List<Transform>();

        foreach (Transform child in gameObject.transform)
        {
            children.Add(child);
        }
    }

    public void TakeDamage()
    {

        sr.color = new Color(255, 255, 255);

        health--;
        if (health == 0)
        {
            Invoke("Die", .5f);
        }

    }

    void Die()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        sr.color = Color.Lerp(sr.color, originalColor, colorLerpSpeed);

        if (Input.GetKeyDown(KeyCode.G))
        {
            TakeDamage();
        }
    }
}
