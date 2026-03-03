using UnityEngine;

[DisallowMultipleComponent]
public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioSource audioSource;

    [Header("SFX")]
    [SerializeField] private AudioClip hitSfx;
    [Range(0f, 1f)][SerializeField] private float hitVolume = 1f;


    private bool launched;


    private void Awake()
    {
        if (!audioSource) audioSource = GetComponent<AudioSource>();
    }

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

    private void OnCollisionEnter(Collision collision)
    {
        // Simple behavior for the jam: destroy on first hit.
        Debug.Log($"Projectile hit {collision.gameObject.name} at {collision.GetContact(0).point}");
        // playsound 
        if (hitSfx != null)
        {
            // Use GetContact to avoid allocations; contact point gives a good 3D audio origin. [web:158][web:159]
            Vector3 p = collision.GetContact(0).point; // contactCount >= 1 on collision. [web:159]
            AudioSource.PlayClipAtPoint(hitSfx, p, hitVolume); // auto-disposes AudioSource. [web:153]
        }

        Destroy(gameObject);
        //Destroy(gameObject);
    }
}
