using UnityEngine;

[CreateAssetMenu(
    fileName = "ProjectileConfig",
    menuName = "VRJam/Weapons/Projectile Config"
)]
public class ProjectileConfig : ScriptableObject
{
    [Header("Launch")]
    [Min(0f)] public float speed = 30f;
    public bool useGravity = false;

    [Header("Reliability (fast bullets)")]
    public CollisionDetectionMode collisionMode = CollisionDetectionMode.ContinuousDynamic;

    [Header("Lifetime")]
    [Min(0.01f)] public float lifetimeSeconds = 5f;
}
