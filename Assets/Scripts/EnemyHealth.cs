using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int health = 3;
    [SerializeField] private float colorLerpSpeed = 5f;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color targetColor = new Color(255, 255, 255);
    [SerializeField] private GameObject noEffectText;
    private Color originalColor;
    private List<Transform> children;
    private bool canBeDamaged = false;

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

    public int GetHealth()
    {
        return health;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "CameraFlash")
        {
            if (canBeDamaged)
            {
                canBeDamaged = false;
                Invoke("ResetVulnerability", 1f); // Basically so they don't die in 1 hit.
                TakeDamage();
            }
            else
            {
                // "No Effect!" 
                Instantiate(noEffectText,
                    new Vector3(transform.position.x, transform.position.y - 1, transform.position.z),
                    Quaternion.identity);
                // You got the hit, but the enemy isn't vulnerable. 
            }
        }
    }

    void ResetVulnerability()
    {
        canBeDamaged = true;
    }

    public void TakeDamage()
    {
        // Visuals
        sr.color = targetColor;
        foreach (Transform child in gameObject.transform)
        {
            if (child.tag != "Shadow")
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

    // Update is called once per frame
    void Update()
    {
        // "Vulnerable during attack" logic
        if (transform.parent.GetComponent<EnemyAttack>().IsAttacking)
        {
            canBeDamaged = true;
        }
        else
        {
            canBeDamaged = false;
        }

        sr.color = Color.Lerp(sr.color, originalColor, colorLerpSpeed);

        foreach (Transform child in gameObject.transform)
        {
            if (child.tag != "Shadow")
            {
                child.gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(sr.color, originalColor, colorLerpSpeed * Time.deltaTime);
            }
        }

        // if (Input.GetKeyDown(KeyCode.G))
        // {
        //     TakeDamage();
        // }
    }
}
