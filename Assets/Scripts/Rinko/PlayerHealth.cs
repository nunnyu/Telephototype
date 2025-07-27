using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int health = 3;
    [SerializeField] private float colorLerpSpeed = .05f;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color targetColor = new Color(255, 255, 255);
    [SerializeField] private float deathDelay = 0.5f;
    [SerializeField] private AudioClip damageSound;
    private Color originalColor;
    private List<Transform> children;
    private bool canBeDamaged = false;
    private int startingHealth;
    private bool picPossible = true; // Corny flag for CanTakePic, we want it toggled, as we shouldn't take a picture
    // while taking damage 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        picPossible = true;
        startingHealth = health;
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

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnReset.AddListener(ReturnToPoint);
            GameManager.Instance.OnReset.AddListener(ResetHealth);
        }
    }

    void ReturnToPoint()
    {
        transform.position = FindAnyObjectByType<GameManager>().rinkoSpawn;
    }

    void ResetHealth()
    {
        health = startingHealth;
    }

    public int GetHealth()
    {
        return health;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyAttack" || other.tag == "MeleeAttack")
        {
            if (canBeDamaged)
            {
                canBeDamaged = false;
                Invoke("ResetVulnerability", 1f); // Basically so they don't die in 1 hit.
                TakeDamage();
            }
        }
    }

    public void TakeDamage()
    {
        // Audio
        GetComponent<PlayerMovement>().ResetAudioCooldown(); // To let our current sound play
        GetComponent<AudioSource>().clip = damageSound; // because we're using the same audiosource
        GetComponent<AudioSource>().Play();

        // Visuals
        sr.color = targetColor;
        foreach (Transform child in children)
        {
            if (child.tag != "Shadow" && child.tag != "Parent")
            {
                child.gameObject.GetComponent<SpriteRenderer>().color = targetColor;
            }
        }

        picPossible = false;
        Invoke("CanTakePic", 1);

        // Health logic
        health--;

        if (health == 0)
        {
            Invoke("Die", deathDelay);
        }
    }

    void CanTakePic()
    {
        picPossible = true;
    }

    void Die()
    {
        GameManager.Instance.TriggerReset();
    }

    void ResetVulnerability()
    {
        canBeDamaged = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        TakePicture.CanTakePicture = picPossible;
        sr.color = Color.Lerp(sr.color, originalColor, colorLerpSpeed * Time.deltaTime);

        foreach (Transform child in children)
        {
            if (child.tag != "Shadow")
            {
                // Fun fact, for like 20 minutes I couldn't figure out why this didn't work
                // and it's because I had csr named sr. 
                var csr = child.gameObject.GetComponent<SpriteRenderer>();
                if (csr)
                {
                    csr.color = Color.Lerp(csr.color, originalColor, colorLerpSpeed * Time.deltaTime);
                }
            }
        }

        Debug.Log(children.Count);

        if (Input.GetKeyDown(KeyCode.G))
        {
            TakeDamage();
        }
    }
}
