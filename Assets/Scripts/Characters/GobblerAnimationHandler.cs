using UnityEngine;

public class GobblerAnimationHandler : MonoBehaviour
{
    public static int gobblerCount;
    public GameObject dialogue;
    private bool counted = false;

    void Start()
    {
        gobblerCount++;
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnReset.AddListener(Reset);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(gobblerCount);

        if (GetComponent<EnemyAttack>().IsAttacking)
        {
            GetComponent<Animator>().SetBool("shoot", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("shoot", false);
        }

        if (GetComponent<EnemyHealth>().GetHealth() == 1)
        {
            if (!counted)
            {
                gobblerCount--;
                counted = true;
            }

            if (gobblerCount == 0)
            {
                Invoke("SummonDialogue", 1);
            }

            // Giving them more health, so they can damage themselves after the dialogue behavior.
            // We don't want the Gobblers to be deactivated and then summon the dialogue, for that doesn't work.
            GetComponent<EnemyHealth>().TakeDamage(); 
        }
    }

    void Reset()
    {
        gobblerCount = 0;
    }

    void SummonDialogue()
    {
        Instantiate(dialogue);
    }
}
