using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int health = 3;
    [SerializeField] private float colorLerpSpeed = 2f;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color targetColor = new Color(255, 255, 255);
    [SerializeField] private GameObject noEffectText;
    [SerializeField] private float deathDelay = 0.5f;
    [SerializeField] private GameObject hitText;
    private Color originalColor;
    private List<Transform> children;
    private bool canBeDamaged = false;
    private int originalHealth;
    private Vector2 originalPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalPosition = transform.position;
        originalHealth = health;
        originalColor = sr.color;

        children = new List<Transform>();

        foreach (Transform child in gameObject.transform)
        {
            children.Add(child);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnReset.AddListener(Reset);
        }
    }

    void Reset()
    {
        health = originalHealth;
        transform.position = originalPosition;
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
                    new Vector3(transform.position.x, transform.position.y - 1.5f, transform.position.z),
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
        // Hit Text
        Instantiate(hitText, new Vector2(transform.position.x + .6f, transform.position.y + 0.5f), Quaternion.identity);

        // Visuals
        sr.color = targetColor;
        foreach (Transform child in children)
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
            Invoke("Die", deathDelay);
        }
    }

    void Die()
    {
        GameManager.spiritsCaptured++;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        EnemyAttack attackScript = transform.GetComponent<EnemyAttack>();
        if (attackScript == null)
        {
            attackScript = transform.parent.GetComponent<EnemyAttack>();
        }

        // "Vulnerable during attack" logic
        if (attackScript != null)
            if (attackScript.IsAttacking)
            {
                canBeDamaged = true;
            }
            else
            {
                canBeDamaged = false;
            }

        sr.color = Color.Lerp(sr.color, originalColor, colorLerpSpeed * Time.deltaTime);

        foreach (Transform child in children)
        {
            if (child.tag != "Shadow")
            {
                child.gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(sr.color, originalColor, colorLerpSpeed * Time.deltaTime);
            }
        }
    }
}
