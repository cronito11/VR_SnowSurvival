using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float amount, GameObject instigator = null);
}
