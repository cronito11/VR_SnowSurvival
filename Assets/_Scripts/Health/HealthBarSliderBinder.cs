using UnityEngine;
using UnityEngine.UI;

public class HealthBarSliderBinder : MonoBehaviour
{
    [SerializeField] private Health target;   // drag Player/NPC Health here
    [SerializeField] private Slider slider;   // drag UI Slider here

    private void OnEnable()
    {
        if (target != null) target.HealthChanged += OnHealthChanged;
        if (target != null) OnHealthChanged(target); // init UI immediately
    }

    private void OnDisable()
    {
        if (target != null) target.HealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(Health h)
    {
        if (slider == null || h == null) return;

        slider.maxValue = h.MaxHealth;
        slider.value = h.CurrentHealth;
    }

    // Optional helper if you want to assign target at runtime
    public void SetTarget(Health newTarget)
    {
        if (target == newTarget) return;

        if (target != null) target.HealthChanged -= OnHealthChanged;
        target = newTarget;
        if (target != null) target.HealthChanged += OnHealthChanged;

        if (target != null) OnHealthChanged(target);
    }
}
