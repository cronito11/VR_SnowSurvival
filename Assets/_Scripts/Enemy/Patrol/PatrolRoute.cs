using UnityEngine;

public class PatrolRoute : MonoBehaviour
{
    public enum RouteMode { Loop }

    [SerializeField] private RouteMode mode = RouteMode.Loop;

    [Tooltip("If empty, you can use 'Auto Fill From Children' to populate from child transforms.")]
    [SerializeField] private Transform[] waypoints;

    public int Count => waypoints == null ? 0 : waypoints.Length;

    public Transform GetWaypoint(int index)
    {
        if (Count == 0) return null;
        index = Mathf.Clamp(index, 0, Count - 1);
        return waypoints[index];
    }

    public int GetNextIndex(int currentIndex)
    {
        if (Count == 0) return 0;

        switch (mode)
        {
            case RouteMode.Loop:
            default:
                return (currentIndex + 1) % Count;
        }
    }

    [ContextMenu("Auto Fill From Children")]
    private void AutoFillFromChildren()
    {
        int c = transform.childCount;
        waypoints = new Transform[c];
        for (int i = 0; i < c; i++)
            waypoints[i] = transform.GetChild(i);
    }

    private void OnDrawGizmosSelected()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        // Draw waypoint spheres + connecting lines for visualization in the Scene view. [web:42]
        Gizmos.color = new Color(1f, 0.7f, 0.1f, 0.9f);

        for (int i = 0; i < waypoints.Length; i++)
        {
            var a = waypoints[i];
            if (!a) continue;

            Gizmos.DrawSphere(a.position, 0.25f);

            int next = (i + 1) % waypoints.Length;
            var b = waypoints[next];
            if (b) Gizmos.DrawLine(a.position, b.position);
        }
    }
}
