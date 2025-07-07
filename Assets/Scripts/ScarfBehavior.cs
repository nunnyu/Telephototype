using UnityEngine;

public class ScarfBehavior : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float angle = 30f;

    // Update is called once per frame
    void Update()
    {
        if (PlayerMovement.Moving)
        {
            float zRotation = Mathf.PingPong(Time.time * speed, angle * 2) - angle;
            transform.rotation = Quaternion.Euler(0, 0, zRotation);
        }
        else
        {
            transform.rotation = Quaternion.identity;
        }

        Debug.Log(PlayerMovement.Moving);
    }
}
