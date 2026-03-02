using UnityEngine;

public class HealthDebugInput : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private float damagePerPress = 10f;
    [SerializeField] private float healPerPress = 10f;

    private void Awake()
    {
        if (health == null) health = GetComponent<Health>();
    }

    private void Update()
    {
        if (health == null) return;

        if (Input.GetKeyDown(KeyCode.D)) health.TakeDamage(damagePerPress);
        if (Input.GetKeyDown(KeyCode.H)) health.Heal(healPerPress);
        if (Input.GetKeyDown(KeyCode.R)) health.Revive(health.MaxHealth);
    }
}
