using UnityEngine;

public class BearEnemyController : MonoBehaviour
{
    private enum State { Patrol, Chase, Attack, Dead }

    [Header("Data")]
    [SerializeField] private EnemyConfig config;

    [Header("Scene refs")]
    [SerializeField] private PatrolRoute patrolRoute;
    [SerializeField] private int startWaypointIndex = 0;

    [Header("Component refs")]
    [SerializeField] private Health health;
    [SerializeField] private BearSenses senses;
    [SerializeField] private BearMovement movement;
    [SerializeField] private BearCombat combat;
    [SerializeField] private BearAnimatorDriver anim;

    [Header("SFX")]
    [SerializeField] private BearSfx sfx;

    private State _state;
    private int _waypointIndex;
    private float _waypointWaitUntil;
    private float _lostTargetTimer;

    private void Reset()
    {
        health = GetComponent<Health>();
        senses = GetComponent<BearSenses>();
        movement = GetComponent<BearMovement>();
        combat = GetComponent<BearCombat>();
        anim = GetComponent<BearAnimatorDriver>();
    }

    private void Awake()
    {
        if (!health) health = GetComponent<Health>();
        if (!senses) senses = GetComponent<BearSenses>();
        if (!movement) movement = GetComponent<BearMovement>();
        if (!combat) combat = GetComponent<BearCombat>();
        if (!anim) anim = GetComponent<BearAnimatorDriver>();
        if (!sfx) sfx = GetComponent<BearSfx>();

        _waypointIndex = Mathf.Max(0, startWaypointIndex);

        if (health != null)
            health.Died += OnDied;
    }

    private void Start()
    {
        if (combat != null && config != null)
            combat.Configure(config.attackDamage, config.attackCooldown);

        EnterPatrol();
    }

    private void OnDestroy()
    {
        if (health != null)
            health.Died -= OnDied;
    }

    private void Update()
    {
        if (_state == State.Dead) return;
        if (config == null) return;

        // Update target acquisition/loss.
        senses?.Tick(config.detectRange, config.loseRange, config.loseSightGraceTime);

        switch (_state)
        {
            case State.Patrol:
                TickPatrol();
                break;
            case State.Chase:
                TickChase();
                break;
            case State.Attack:
                TickAttack();
                break;
        }

        // Feed animator speed from actual movement.
        if (anim != null && movement != null && config.chaseSpeed > 0.01f)
        {
            float speed01 = Mathf.Clamp01(movement.Velocity.magnitude / config.chaseSpeed);
            anim.SetMoveSpeed(speed01);
        }
    }

    private void TickPatrol()
    {
        // If player appears, go chase.
        if (senses != null && senses.HasTarget)
        {
            EnterChase();
            return;
        }

        if (patrolRoute == null || patrolRoute.Count == 0)
        {
            movement?.Stop();
            return;
        }

        if (Time.time < _waypointWaitUntil) return;

        movement?.SetSpeed(config.patrolSpeed);

        Transform wp = patrolRoute.GetWaypoint(_waypointIndex);
        if (wp != null)
            movement?.MoveTo(wp.position);

        if (movement != null && movement.HasReachedDestination(config.waypointReachDistance))
        {
            _waypointIndex = patrolRoute.GetNextIndex(_waypointIndex);
            _waypointWaitUntil = Time.time + Mathf.Max(0f, config.waypointWaitTime);
        }
    }

    private void TickChase()
    {
        if (senses == null || !senses.HasTarget)
        {
            EnterPatrol();
            return;
        }

        Transform target = senses.Target;

        movement?.SetSpeed(config.chaseSpeed);
        movement?.MoveTo(target.position); // Requests/updates a path toward target. [page:10]

        if (senses.IsInAttackRange(config.attackRange))
        {
            EnterAttack();
            return;
        }
    }

    private void TickAttack()
    {
        if (senses == null || !senses.HasTarget)
        {
            EnterPatrol();
            return;
        }

        Transform target = senses.Target;

        // If target moved away, chase again.
        if (!senses.IsInAttackRange(config.attackRange))
        {
            EnterChase();
            return;
        }

        // Hold position, face player, try to attack on cooldown.
        movement?.Stop();
        movement?.FaceTowards(target.position);

        combat?.TryStartAttack();
    }

    private void EnterPatrol()
    {
        _state = State.Patrol;
        _lostTargetTimer = 0f;
        sfx?.ResetDetect();
    }

    private void EnterChase()
    {
        _state = State.Chase;
        _lostTargetTimer = 0f;
        sfx?.OnDetectPlayer();
    }

    private void EnterAttack()
    {
        _state = State.Attack;
        movement?.Stop();
    }

    private void OnDied(Health h)
    {
        _state = State.Dead;

        // Play sound first (in case you disable components after).
        sfx?.OnDeath();

        // Stop motion + play death.
        movement?.Stop();
        anim?.SetDead(true);

        // Optional: disable sensing/combat to avoid extra calls.
        if (senses) senses.enabled = false;
        if (combat) combat.enabled = false;

        // Optional: destroy after delay (keep if you want loot/ragdoll etc).
        if (config != null && config.despawnDelay > 0f)
            Destroy(gameObject, config.despawnDelay);

    }
    public void SetExternalWiring(EnemyConfig config, PatrolRoute route)
    {
        if (config != null) this.config = config;
        if (route != null) this.patrolRoute = route;
    }

    public void AE_PlayAttackSfx()
    {
        GetComponent<BearSfx>()?.OnAttack();
    }

    //public void AE_AttackSfx() => OnAttack();

}
