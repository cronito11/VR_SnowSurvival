using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BearAnimatorDriver : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;

    [Header("Parameter names (must match Animator)")]
    [SerializeField] private string speedParam = "Speed";
    [SerializeField] private string movingParam = "IsMoving";
    [SerializeField] private string attackTrigger = "Attack";
    [SerializeField] private string hitTrigger = "Hit";
    [SerializeField] private string deadBool = "IsDead";

    [Header("Smoothing")]
    [SerializeField] private float speedDampTime = 0.08f;

    private int _speedId;
    private int _movingId;
    private int _attackId;
    private int _hitId;
    private int _deadId;

    private void Reset()
    {
        animator = GetComponent<Animator>();
    }

    private void Awake()
    {
        if (!animator) animator = GetComponent<Animator>();

        // Hash parameter strings once for cheaper set calls. [page:11]
        _speedId = Animator.StringToHash(speedParam);
        _movingId = Animator.StringToHash(movingParam);
        _attackId = Animator.StringToHash(attackTrigger);
        _hitId = Animator.StringToHash(hitTrigger);
        _deadId = Animator.StringToHash(deadBool);
    }

    public void SetMoveSpeed(float speed01)
    {
        if (!animator) return;

        // Damped SetFloat helps avoid jitter when speed changes rapidly. [page:9]
        animator.SetFloat(_speedId, speed01, speedDampTime, Time.deltaTime);
        animator.SetBool(_movingId, speed01 > 0.05f);
    }

    public void TriggerAttack()
    {
        if (!animator) return;
        animator.SetTrigger(_attackId);
    }

    public void TriggerHit()
    {
        if (!animator) return;
        animator.SetTrigger(_hitId);
    }

    public void SetDead(bool isDead)
    {
        if (!animator) return;
        animator.SetBool(_deadId, isDead);
    }
}
