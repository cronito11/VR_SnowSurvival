using UnityEngine;

public enum FireMode
{
    SemiAuto,
    FullAuto
}

[DisallowMultipleComponent]
public class SimpleShooter : MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField] private Transform muzzle;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private ProjectileConfig projectileConfig;

    [Header("Firing")]
    [SerializeField] private FireMode fireMode = FireMode.SemiAuto;

    [Tooltip("Only used for FullAuto. Example: 10 = 10 shots/sec.")]
    [Min(0.1f)]
    [SerializeField] private float shotsPerSecond = 10f;

    private bool triggerHeld;
    private float nextShotTime;

    private void OnValidate()
    {
        if (muzzle == null) muzzle = transform;
        if (shotsPerSecond < 0.1f) shotsPerSecond = 0.1f;
    }

    private void Update()
    {
        if (fireMode != FireMode.FullAuto) return;
        if (!triggerHeld) return;

        TryShoot();
    }

    // Call this from input (keyboard now, VR trigger later)
    public void TriggerDown()
    {
        if (fireMode == FireMode.SemiAuto)
        {
            ShootOnce();
            return;
        }

        triggerHeld = true;
        TryShoot(); // fire immediately on press
    }

    public void TriggerUp()
    {
        triggerHeld = false;
    }

    private void TryShoot()
    {
        // Rate-limit using Time.time + interval (nextFire pattern). [web:108]
        if (Time.time < nextShotTime) return;

        nextShotTime = Time.time + (1f / shotsPerSecond); // shotsPerSecond -> seconds per shot. [web:108]
        ShootOnce();
    }

    private void ShootOnce()
    {
        if (projectilePrefab == null || projectileConfig == null || muzzle == null)
            return;

        Projectile projectile = Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);
        projectile.Launch(muzzle.forward, projectileConfig);
    }
}
