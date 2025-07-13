using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float lerpSpeed = 5f;

    // Update is called once per frame
    void FixedUpdate()
    {
        var target = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, target, lerpSpeed * Time.deltaTime);
    }
}
