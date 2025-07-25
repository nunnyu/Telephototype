using UnityEngine;

[System.Serializable]
public class BodyPartAnimators
{
    public Animator body;
    public Animator leftWing;
    public Animator leftGiblet;
    public Animator rightWing;
    public Animator rightGiblet;
}

public class MasterAnimationController : MonoBehaviour
{
    [Header("Body Parts")]
    public BodyPartAnimators animators;

    [Header("Layer Settings")]
    [SerializeField] private string defaultLayer = "Characters";
    [SerializeField] private string frontLayer = "Foreground";
    [SerializeField] private float layerTransitionSpeed = 8f;

    private Vector3 targetPosition;
    private float defaultSortingOrder;

    void Start()
    {
        targetPosition = new Vector3(transform.position.x + 0.2f, transform.position.y, transform.position.z);
    }

    private void Update()
    {
        HandleLeftWingOrder();
        HandleGibletOrder();

        transform.position = Vector3.Lerp(transform.position, targetPosition, 2f * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Invoke("Method", 0.1f);
        }
    }

    // ??? why did you name this Method sob 
    private void Method()
    {
        var animator = GetComponent<Animator>();
        if (animator.GetBool("attacking"))
        {
            GoBackToDefault();
        }
        else if (animator.GetBool("alert"))
        {
            // Add alert behavior here
        }
        else
        {
            GoBackToDefault();
        }
    }

    private void GoBackToDefault()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, 2f * Time.deltaTime);
    }

    public void SetState(string state)
    {
        switch (state.ToLower())
        {
            case "idle":
                SetWingBehind();
                SetGibletBehind();
                break;

            case "alert":
                SetWingBehind();
                SetGibletBehind();
                break;

            case "attack":
                SetWingForward();
                SetGibletForward();
                break;
        }

        animators.body.SetTrigger(state);
        animators.leftWing.SetTrigger(state);
        animators.rightWing.SetTrigger(state);
        animators.leftGiblet.SetTrigger(state);
        animators.rightGiblet.SetTrigger(state);
    }

    private void HandleLeftWingOrder()
    {
        var wingRenderer = animators.leftWing.GetComponent<SpriteRenderer>();
        string targetLayer = animators.body.GetBool("attack") ? frontLayer : defaultLayer;

        if (wingRenderer.sortingLayerName != targetLayer)
        {
            wingRenderer.sortingLayerName = targetLayer;
            wingRenderer.sortingOrder = Mathf.RoundToInt(Mathf.Lerp(
                wingRenderer.sortingOrder,
                targetLayer == frontLayer ? 2 : 0,
                Time.deltaTime * layerTransitionSpeed
            ));
        }
    }

    private void HandleGibletOrder()
    {
        var gibletRenderer = animators.leftGiblet.GetComponent<SpriteRenderer>();
        bool shouldBeInFront = animators.body.GetBool("IsAttacking") &&
                             animators.leftGiblet.GetCurrentAnimatorStateInfo(0).IsName("Attack");

        gibletRenderer.sortingLayerName = shouldBeInFront ? frontLayer : defaultLayer;
    }

    private void SetWingBehind() => SetPartOrder(animators.leftWing, defaultLayer, 0);
    private void SetWingForward() => SetPartOrder(animators.leftWing, frontLayer, 2);
    private void SetGibletBehind() => SetPartOrder(animators.leftGiblet, defaultLayer, 0);
    private void SetGibletForward() => SetPartOrder(animators.leftGiblet, frontLayer, 1);

    private void SetPartOrder(Animator partAnimator, string layer, int order)
    {
        var renderer = partAnimator.GetComponent<SpriteRenderer>();
        renderer.sortingLayerName = layer;
        renderer.sortingOrder = order;
    }
}