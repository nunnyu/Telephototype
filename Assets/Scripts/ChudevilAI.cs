using System.Collections;
using UnityEngine;

public class ChudevilAI : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 3f;
    [SerializeField] private Vector2 detectionOffset = new Vector2(0, 0);
    [SerializeField] private float lowestDelay = 3f;
    [SerializeField] private float highestDelay = 4f;
    private bool attacking = false;

    bool PlayerInRange()
    {
        Vector2 center = new Vector2(transform.position.x + detectionOffset.x, transform.position.y + detectionOffset.y);
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, detectionRadius);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player") || hit.CompareTag("CameraPoses"))
            {
                return true;
            }
        }

        return false;
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
        else
        {
            attacking = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x + detectionOffset.x, transform.position.y + detectionOffset.y), detectionRadius);
    }
}
