using System;
using UnityEngine;

public enum ZeroHealthAction { KnockOut, Die }
public enum HealthState { Alive, KnockedOut, Dead }

public class Health : MonoBehaviour,IDamageable
{
    [Header("Config")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private ZeroHealthAction onZeroHealth = ZeroHealthAction.Die;

    public float MaxHealth => maxHealth;
    [field:SerializeField] public float CurrentHealth { get; private set; }
    public float NormalizedHealth => MaxHealth <= 0f ? 0f : CurrentHealth / MaxHealth;
    public HealthState State { get; private set; } = HealthState.Alive;

    // UI/dev hooks (bind later)
    public event Action<Health> HealthChanged;      // fires on any health change
    public event Action<Health> KnockedOut;         // fires once when KO happens
    public event Action<Health> Died;               // fires once when death happens

    private void Awake()
    {
        CurrentHealth = Mathf.Clamp(maxHealth, 0f, float.MaxValue);
        State = HealthState.Alive;
        HealthChanged?.Invoke(this);
    }

   

    public void Heal(float amount)
    {
        if (State == HealthState.Dead) return; // change if you want resurrection via Heal()
        if (amount <= 0f) return;

        SetHealth(CurrentHealth + amount);
    }

    public void Revive(float healthAmount = 1f)
    {
        State = HealthState.Alive;
        SetHealth(Mathf.Max(healthAmount, 1f));
    }

    public void TakeDamage(float amount, GameObject instigator = null)
    {

        if (State != HealthState.Alive) return;
        if (amount <= 0f) return;

        SetHealth(CurrentHealth - amount);

        if (CurrentHealth <= 0f)
            HandleZeroHealth();
    }

    public void TakeDamage(float amount)
    {
        TakeDamage(amount, null);
    }

    public void SetMaxHealth(float newMax, bool keepPercent = true)
    {
        newMax = Mathf.Max(0f, newMax);

        float percent = NormalizedHealth;
        maxHealth = newMax;

        CurrentHealth = keepPercent ? percent * maxHealth : Mathf.Min(CurrentHealth, maxHealth);
        HealthChanged?.Invoke(this);
    }

    private void SetHealth(float newHealth)
    {
        float clamped = Mathf.Clamp(newHealth, 0f, MaxHealth);
        if (Mathf.Approximately(clamped, CurrentHealth)) return;

        CurrentHealth = clamped;
        HealthChanged?.Invoke(this);
    }

    private void HandleZeroHealth()
    {
        if (onZeroHealth == ZeroHealthAction.Die)
        {
            State = HealthState.Dead;
            Died?.Invoke(this);
        }
        else
        {
            State = HealthState.KnockedOut;
            KnockedOut?.Invoke(this);
        }
    }

}
