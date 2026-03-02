using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BearMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent agent;

    [Header("Turning")]
    [SerializeField] private float faceTurnSpeed = 720f; // degrees/sec

    public Vector3 Velocity => agent ? agent.velocity : Vector3.zero;

    private void Reset()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Awake()
    {
        if (!agent) agent = GetComponent<NavMeshAgent>();
    }

    public void SetSpeed(float speed)
    {
        if (!agent) return;
        agent.speed = speed;
    }

    public bool MoveTo(Vector3 worldPos)
    {
        if (!agent) return false;

        agent.isStopped = false; // resumes movement along current/new path. [web:72]
        return agent.SetDestination(worldPos); // requests a path; may be pending for a few frames. [page:10]
    }

    public void Stop()
    {
        if (!agent) return;

        agent.isStopped = true; // stops movement along its current path. [web:72]
        agent.ResetPath();
    }

    public bool HasReachedDestination(float extraTolerance = 0.0f)
    {
        if (!agent) return true;
        if (agent.pathPending) return false;

        // remainingDistance is the distance along the current path (can be infinity if unknown). [web:67]
        float threshold = agent.stoppingDistance + extraTolerance;
        return agent.remainingDistance <= threshold;
    }

    public void FaceTowards(Vector3 worldPos)
    {
        Vector3 to = worldPos - transform.position;
        to.y = 0f;
        if (to.sqrMagnitude < 0.0001f) return;

        Quaternion targetRot = Quaternion.LookRotation(to, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRot,
            faceTurnSpeed * Time.deltaTime
        );
    }
}
