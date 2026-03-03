using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [Header("What to spawn")]
    [SerializeField] private GameObject enemyPrefab;

    [Header("Wiring (optional overrides)")]
    [SerializeField] private EnemyConfig configOverride;
    [SerializeField] private PatrolRoute patrolRoute;

    [Header("Spawn behavior")]
    [SerializeField] private bool spawnOnStart = true;
    [SerializeField] private bool respawn = false;
    [SerializeField] private float respawnDelay = 10f;

    private GameObject _current;

    private void Start()
    {
        if (spawnOnStart)
            Spawn();
    }

    public void Spawn()
    {
        if (!enemyPrefab) return;
        if (_current) return;

        _current = Instantiate(enemyPrefab, transform.position, transform.rotation); // spawns prefab instance. [web:199]

        // Wire bear controller if present (works for any enemy using this controller pattern).
        var controller = _current.GetComponent<BearEnemyController>();
        if (controller != null)
        {
            controller.SetExternalWiring(
                configOverride,
                patrolRoute
            );
        }

        if (respawn)
        {
            var health = _current.GetComponent<Health>();
            if (health != null)
            {
                health.Died += OnSpawnedEnemyDied;
            }
        }
    }

    private void OnSpawnedEnemyDied(Health h)
    {
        if (_current == null) return;

        h.Died -= OnSpawnedEnemyDied;

        // The bear destroys itself after its own despawnDelay,
        // so we just schedule a respawn from the spawn point.
        _current = null;
        Invoke(nameof(Spawn), Mathf.Max(0f, respawnDelay));
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.2f, 1f, 0.2f, 0.9f);
        Gizmos.DrawWireSphere(transform.position, 0.4f); // spawn marker when selected. [web:208]
    }
#endif
}
