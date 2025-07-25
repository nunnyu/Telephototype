using System.Collections;
using UnityEngine;

public class GenericEnemyAttackAI : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 3f;
    [SerializeField] private Vector2 detectionOffset = new Vector2(0, 0);
    [SerializeField] private float lowestDelay = 3f;
    [SerializeField] private float highestDelay = 4f;
    private bool isAttackingCoroutineRunning = false;

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


    void Update()
    {
        if (PlayerInRange())
        {
            Debug.Log("Rock attacking");
            if (!isAttackingCoroutineRunning)
            {
                StartCoroutine(RandomAttackLoop());
            }
        }
    }

    IEnumerator RandomAttackLoop()
    {
        isAttackingCoroutineRunning = true;
        GetComponent<EnemyAttack>().Attack();
        float waitTime = UnityEngine.Random.Range(lowestDelay, highestDelay);
        yield return new WaitForSeconds(waitTime);
        isAttackingCoroutineRunning = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x + detectionOffset.x, transform.position.y + detectionOffset.y), detectionRadius);
    }
}
