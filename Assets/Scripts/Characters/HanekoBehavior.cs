using System;
using System.Collections;
using UnityEngine;

public class HanekoBehavior : MonoBehaviour
{
    [SerializeField] private float lowestDelay = 3f;
    [SerializeField] private float highestDelay = 5f;
    [SerializeField] private EnemyHealth health;
    [SerializeField] private GameObject endDialogue;
    private bool Attacking = false;
    private bool isAttackingCoroutineRunning = false;

    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnTutorialFightStart.AddListener(StartAttacking);
            GameManager.Instance.OnTutorialFightEnd.AddListener(StopAttacking);
        }
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnTutorialFightStart.RemoveListener(StartAttacking);
            GameManager.Instance.OnTutorialFightEnd.RemoveListener(StopAttacking);
        }
    }

    void StopAttacking()
    {
        Attacking = false;
    }

    void StartAttacking()
    {
        Debug.Log("Haneko got the START ATTACK event!");
        Attacking = true;
    }

    void Update()
    {
        int healthNum = health.GetHealth();
        if (healthNum == 0 && Attacking)
        {
            GameManager.Instance.TriggerTutorialFightEnd();
            Debug.Log("Triggering end of tutorial.");
            StartCoroutine(SpawnEndWithDelay(2));
        }

        Debug.Log("Attacking: " + Attacking);
        if (Attacking && !isAttackingCoroutineRunning)
        {
            StartCoroutine(RandomAttackLoop());
        }
    }

    IEnumerator SpawnEndWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(endDialogue, transform.position, Quaternion.identity, null);
    }


    IEnumerator RandomAttackLoop()
    {
        isAttackingCoroutineRunning = true;
        while (Attacking)
        {
            GetComponent<EnemyAttack>().Attack();
            float waitTime = UnityEngine.Random.Range(lowestDelay, highestDelay);
            yield return new WaitForSeconds(waitTime);
        }
        isAttackingCoroutineRunning = false;
    }
}