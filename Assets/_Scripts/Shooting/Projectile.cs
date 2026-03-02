using UnityEngine;

[DisallowMultipleComponent]
public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    private bool launched;

    private void Reset()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Launch(Vector3 direction, ProjectileConfig config)
    {
        if (launched) return;
        launched = true;

        rb.useGravity = config.useGravity;
        rb.collisionDetectionMode = config.collisionMode; // helps with fast bullets [web:20]

        // We set initial velocity once; Unity warns against setting velocity every physics step. [web:53]
        rb.linearVelocity = direction.normalized * config.speed; // velocity is world-space. [web:53]

        Destroy(gameObject, config.lifetimeSeconds);
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        // Simple behavior for the jam: destroy on first hit.
        Destroy(gameObject);
    }*/
}
