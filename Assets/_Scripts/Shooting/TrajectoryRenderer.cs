using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryRenderer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform muzzle;
    [SerializeField] private ProjectileConfig projectileConfig;

    [Header("Trajectory")]
    [Min(2)][SerializeField] private int segments = 30;
    [Min(0.005f)][SerializeField] private float timeStep = 0.05f;

    [Header("Collision (optional)")]
    [SerializeField] private bool stopAtHit = true;
    [SerializeField] private LayerMask hitMask = ~0;
    [SerializeField] private QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Ignore;

    [Header("Visibility")]
    [SerializeField] private bool visible = true;

    private LineRenderer lr;
    private Vector3[] points;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        points = new Vector3[segments];
        ApplyVisibility();
    }

    private void OnValidate()
    {
        if (segments < 2) segments = 2;
        if (timeStep < 0.005f) timeStep = 0.005f;
    }

    private void Update()
    {
        if (!visible || muzzle == null || projectileConfig == null) return;

        if (points == null || points.Length != segments)
            points = new Vector3[segments];

        Vector3 startPos = muzzle.position;
        Vector3 startVel = muzzle.forward * projectileConfig.speed;
        Vector3 gravity = projectileConfig.useGravity ? Physics.gravity : Vector3.zero;

        int count = 0;
        Vector3 prev = startPos;

        for (int i = 0; i < segments; i++)
        {
            float t = i * timeStep;
            Vector3 pos = startPos + startVel * t + 0.5f * gravity * (t * t);

            if (i > 0 && stopAtHit)
            {
                Vector3 dir = pos - prev;
                float dist = dir.magnitude;

                if (dist > 0.0001f && Physics.Raycast(prev, dir / dist, out RaycastHit hit, dist, hitMask, triggerInteraction))
                {
                    points[count++] = hit.point;
                    break; // stop the line at first hit
                }
            }

            points[count++] = pos;
            prev = pos;
        }

        lr.positionCount = count;              // set vertex count first [web:116]
        lr.SetPositions(points);               // ignores indices beyond positionCount [web:116]
    }

    public void SetVisible(bool isVisible)
    {
        visible = isVisible;
        ApplyVisibility();
    }

    private void ApplyVisibility()
    {
        if (lr != null) lr.enabled = visible;
    }
}
