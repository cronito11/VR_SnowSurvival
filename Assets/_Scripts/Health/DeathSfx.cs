using UnityEngine;

public class DeathSfx : MonoBehaviour
{
    [SerializeField] private Health health;        // same object usually
    [SerializeField] private AudioClip deathClip;
    [SerializeField, Range(0f, 1f)] private float volume = 1f;

    private void Awake()
    {
        if (health == null) health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        if (health != null) health.Died += OnDied;
    }

    private void OnDisable()
    {
        if (health != null) health.Died -= OnDied;
    }

    private void OnDied(Health h)
    {
        if (deathClip == null) return;
        AudioSource.PlayClipAtPoint(deathClip, transform.position, volume);
    }
}
