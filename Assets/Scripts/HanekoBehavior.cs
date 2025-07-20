using System;
using System.Collections;
using UnityEngine;

public class HanekoBehavior : MonoBehaviour
{
    private bool Attacking = false;
    private bool isAttackingCoroutineRunning = false;

    void Start()
    {
        GameManager.OnTutorialFightStart += StartAttacking;
        GameManager.OnTutorialFightEnd += StopAttacking;
    }

    void StopAttacking()
    {
        Attacking = false;
    }

    void StartAttacking()
    {
        Attacking = true;
    }

    void OnDestroy()
    {
        GameManager.OnTutorialFightStart -= StartAttacking;
    }

    void Update()
    {
        if (Attacking && !isAttackingCoroutineRunning)
        {
            StartCoroutine(RandomAttackLoop());
        }
    }

    IEnumerator RandomAttackLoop()
    {
        isAttackingCoroutineRunning = true;
        while (Attacking)
        {
            GetComponent<EnemyAttack>().Attack();
            float waitTime = UnityEngine.Random.Range(1f, 3f);
            yield return new WaitForSeconds(waitTime);
        }
        isAttackingCoroutineRunning = false;
    }
}