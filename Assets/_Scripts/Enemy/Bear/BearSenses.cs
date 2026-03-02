using UnityEngine;

public class BearSenses : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform eyes; // optional, can be null (falls back to transform)

    [Header("Targeting")]
    [SerializeField] private LayerMask targetMask; // put Player on its own layer and select it here

    [Header("Debug")]
    [SerializeField] private bool drawGizmos = true;

    private readonly Collider[] _hits = new Collider[8];
    private Transform _target;
    private float _timeSinceSeen;

    public Transform Target => _target;

    public bool HasTarget => _target != null;

    public void ClearTarget()
    {
        _target = null;
        _timeSinceSeen = 0f;
    }

    // Call this once per frame from the controller.
    public void Tick(float detectRange, float loseRange, float loseGraceTime)
    {
        Vector3 origin = eyes ? eyes.position : transform.position;

        if (_target == null)
        {
            AcquireTarget(origin, detectRange);
            return;
        }

        float dist = Vector3.Distance(origin, _target.position);

        if (dist <= loseRange)
        {
            _timeSinceSeen = 0f;
            return;
        }

        _timeSinceSeen += Time.deltaTime;
        if (_timeSinceSeen >= loseGraceTime)
            ClearTarget();
    }

    private void AcquireTarget(Vector3 origin, float detectRange)
    {
        // NonAlloc avoids garbage; it stores colliders into our preallocated buffer. [page:8]
        int count = Physics.OverlapSphereNonAlloc(
            origin,
            detectRange,
            _hits,
            targetMask,
            QueryTriggerInteraction.Ignore
        );

        if (count <= 0) return;

        // Pick the closest.
        Transform best = null;
        float bestDist = float.PositiveInfinity;

        for (int i = 0; i < count; i++)
        {
            var c = _hits[i];
            if (!c) continue;

            float d = Vector3.Distance(origin, c.transform.position);
            if (d < bestDist)
            {
                bestDist = d;
                best = c.transform;
            }
        }

        _target = best;
        _timeSinceSeen = 0f;
    }

    public bool IsInAttackRange(float attackRange)
    {
        if (_target == null) return false;

        Vector3 origin = eyes ? eyes.position : transform.position;
        return Vector3.Distance(origin, _target.position) <= attackRange;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (!drawGizmos) return;
        Gizmos.color = new Color(1f, 0f, 0f, 0.25f);

        // Note: ranges come from EnemyConfig at runtime; this is just a visual hint if you want.
        Vector3 origin = eyes ? eyes.position : transform.position;
        Gizmos.DrawSphere(origin, 0.25f);
    }
#endif
}
