using UnityEngine;

public class GobblerAnimationHandler : MonoBehaviour
{
    public static int gobblerCount;

    void Start()
    {
        gobblerCount++;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<EnemyAttack>().IsAttacking)
        {
            GetComponent<Animator>().SetBool("shoot", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("shoot", false);
        }
    }
}
