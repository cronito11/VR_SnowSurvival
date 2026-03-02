using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    [SerializeField] private float damage = 25f;
    [SerializeField] private GameObject owner; // who fired (optional)

    private void OnTriggerEnter(Collider other)
    {
        // If you use separate child hitboxes, this is usually safer:
        var damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
            damageable.TakeDamage(damage, owner);
        else
            Debug.LogWarning($"Projectile hit {other.name} but it has no IDamageable component!");
        Destroy(gameObject);
    }
}
