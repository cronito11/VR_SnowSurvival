using UnityEngine;

[DisallowMultipleComponent]
public class SimpleShooter : MonoBehaviour
{
    [SerializeField] private Transform muzzle;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private ProjectileConfig projectileConfig;

    private void OnValidate()
    {
        if (muzzle == null) muzzle = transform;
    }

    public void Shoot()
    {
        if (projectilePrefab == null || projectileConfig == null || muzzle == null)
            return;

        Projectile projectile = Instantiate(
            projectilePrefab,
            muzzle.position,
            muzzle.rotation
        ); // position + rotation are world-space. [web:63]

        projectile.Launch(muzzle.forward, projectileConfig); // uses muzzle forward direction. [web:76]
    }
}
