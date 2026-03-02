using UnityEngine;

public class BearCombat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BearAnimatorDriver animDriver;

    [Header("Hit settings")]
    [SerializeField] private Transform hitOrigin;          // e.g., mouth/claw point (optional)
    [SerializeField] private float hitRadius = 1.2f;
    [SerializeField] private LayerMask hittableMask;       // usually Player layer
    [SerializeField] private bool ignoreTriggers = true;

    [Header("Audio")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip attackSfx;
    [Range(0f, 1f)][SerializeField] private float attackSfxVolume = 1f;

    private readonly Collider[] _hits = new Collider[8];

    private float _nextAttackTime;
    private float _damage;
    private float _cooldown;

    public void Awake()
    {
        if (!animDriver) animDriver = GetComponent<BearAnimatorDriver>();
        if (!sfxSource) sfxSource = GetComponent<AudioSource>();
    }

    public void Configure(float damage, float cooldownSeconds)
    {
        _damage = damage;
        _cooldown = cooldownSeconds;
    }

    public bool CanAttackNow => Time.time >= _nextAttackTime;

    public bool TryStartAttack()
    {
        if (!CanAttackNow) return false;

        _nextAttackTime = Time.time + _cooldown;

        if (!animDriver) animDriver = GetComponent<BearAnimatorDriver>();
        animDriver?.TriggerAttack();

        return true;
    }

    // Animation Event: call this at the "hit frame" of the Attack clip.
    // (No parameters needed; if you *do* want one, it must be only 1 param type Unity supports.) [page:12]
    public void AE_ApplyAttackDamage()
    {
        Vector3 origin = hitOrigin ? hitOrigin.position : transform.position + transform.forward * 1.0f;

        int count = Physics.OverlapSphereNonAlloc(
            origin,
            hitRadius,
            _hits,
            hittableMask,
            ignoreTriggers ? QueryTriggerInteraction.Ignore : QueryTriggerInteraction.Collide
        ); // Stores colliders into our buffer and returns how many were written. [page:13]

        for (int i = 0; i < count; i++)
        {
            var c = _hits[i];
            if (!c) continue;

            // Simple + generic: anything that has a Health component can take damage.
            // We’ll wire this to your existing health mechanism next.
            var health = c.GetComponentInParent<Health>();
            if (health != null)
                health.TakeDamage(_damage);
        }
    }

    public void AE_PlayAttackSfx()
    {
        if (!sfxSource || !attackSfx) return;
        sfxSource.PlayOneShot(attackSfx, attackSfxVolume); // one-shot, doesn’t cancel others. [page:14]
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Vector3 origin = hitOrigin ? hitOrigin.position : transform.position + transform.forward * 1.0f;
        Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
        Gizmos.DrawSphere(origin, hitRadius);
    }
#endif
}
