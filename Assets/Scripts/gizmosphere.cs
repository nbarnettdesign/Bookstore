using UnityEngine;

public class ExampleScript : MonoBehaviour
{
    public Color gizmoColor = Color.yellow;
    public float gizmoSize = 0.5f;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, gizmoSize);
    }
}
