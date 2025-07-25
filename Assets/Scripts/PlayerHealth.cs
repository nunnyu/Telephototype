using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int health = 3;
    [SerializeField] private float colorLerpSpeed = .05f;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color targetColor = new Color(255, 255, 255);
    private Color originalColor;
    private List<Transform> children;
    private bool canBeDamaged = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canBeDamaged = true;
        originalColor = sr.color;

        children = new List<Transform>();

        foreach (Transform child in gameObject.transform)
        {
            if (child.tag == "Parent")
            {
                children.Add(child.transform.GetChild(0));
            }
            else
            {
                children.Add(child);
            }
        }
    }

    public int GetHealth()
    {
        return health;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyAttack")
        {
            if (canBeDamaged)
            {
                canBeDamaged = false;
                Invoke("ResetVulnerability", 1f); // Basically so they don't die in 1 hit.
                TakeDamage();
            }

            Destroy(other);
        }
    }

    public void TakeDamage()
    {
        // Visuals
        sr.color = targetColor;
        foreach (Transform child in children)
        {
            if (child.tag != "Shadow" && child.tag != "Parent")
            {
                child.gameObject.GetComponent<SpriteRenderer>().color = targetColor;
            }
        }

        // Health logic
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

    void ResetVulnerability()
    {
        canBeDamaged = true;
    }

    // Update is called once per frame
    void Update()
    {
        sr.color = Color.Lerp(sr.color, originalColor, colorLerpSpeed);

        foreach (Transform child in children)
        {
            if (child.tag != "Shadow")
            {
                var sr = child.gameObject.GetComponent<SpriteRenderer>();
                if (sr)
                {
                    sr.color = Color.Lerp(sr.color, originalColor, colorLerpSpeed * Time.deltaTime);
                }
            }
        }
    }
}
