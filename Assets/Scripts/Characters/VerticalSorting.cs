using UnityEngine;
using UnityEngine.Rendering;

public class VerticalSorting : MonoBehaviour
{
    [SerializeField] private Transform primaryObject;
    [SerializeField] private float offset = 0;
    private SortingGroup sortingGroup;

    void Awake()
    {
        sortingGroup = GetComponent<SortingGroup>();
    }

    void LateUpdate()
    {
        if (primaryObject)
        {
            sortingGroup.sortingOrder = Mathf.RoundToInt((-primaryObject.position.y + offset) * 100);
        }
    }

    void OnDrawGizmos()
    {
        if (primaryObject)
        {
            Vector3 positionForGizmo = new Vector3(primaryObject.position.x, -primaryObject.position.y + offset, primaryObject.position.z);
            Gizmos.DrawSphere(positionForGizmo, 0.1f);
        }
    }
}
