using System.Collections;
using UnityEngine;

public class ChudevilAI : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 3f;
    [SerializeField] private Vector2 detectionAreaPos = new Vector2(0, 0);
    [SerializeField] private GameObject dialogue;
    private bool attacking = false;

    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnReset.AddListener(Reset);
        }
    }

    void Reset()
    {
        // attacking = false; 
        Destroy(gameObject);
    }

    bool PlayerInRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionAreaPos, detectionRadius);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player") /*|| hit.CompareTag("CameraPoses")*/)
            {
                return true;
            }
        }

        return false;
    }

    void SpawnDialogue()
    {
        Instantiate(dialogue);
    }

    public bool IsAttacking()
    {
        return attacking;
    }

    void Update()
    {
        if (PlayerInRange())
        {
            attacking = true;
        }

        GetComponent<EnemyAttack>().IsAttacking = attacking;

        if (GetComponent<EnemyHealth>().GetHealth() == 0)
        {
            Invoke("SpawnDialogue", 3);
            ChudevilSpawner.chudevilDead = true;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectionAreaPos, detectionRadius);
    }
}
