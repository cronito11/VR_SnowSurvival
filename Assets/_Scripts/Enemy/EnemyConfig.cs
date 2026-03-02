using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig_Bear", menuName = "Game/Enemies/Enemy Config", order = 0)]
public class EnemyConfig : ScriptableObject
{
    [Header("Movement")]
    [Min(0.1f)] public float patrolSpeed = 2.0f;
    [Min(0.1f)] public float chaseSpeed = 4.0f;
    [Min(0.0f)] public float waypointReachDistance = 0.6f;
    [Min(0.0f)] public float waypointWaitTime = 0.0f;

    [Header("Senses")]
    [Min(0.0f)] public float detectRange = 10.0f;
    [Min(0.0f)] public float attackRange = 2.0f;
    [Min(0.0f)] public float loseRange = 14.0f;
    [Min(0.0f)] public float loseSightGraceTime = 1.0f;

    [Header("Combat")]
    [Min(0.0f)] public float attackCooldown = 1.2f;
    [Min(0)] public float attackDamage = 15;

    [Header("Death")]
    [Min(0.0f)] public float despawnDelay = 5.0f;

    private void OnValidate()
    {
        if (loseRange < detectRange) loseRange = detectRange;
        if (attackRange > detectRange) attackRange = detectRange;
    }
}
