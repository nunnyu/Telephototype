using System.Collections;
using UnityEngine;

public class ChudevilAI : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 3f;
    [SerializeField] private Vector2 detectionAreaPos = new Vector2(0, 0);
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
        attacking = false;
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
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectionAreaPos, detectionRadius);
    }
}
