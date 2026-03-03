using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class UI_LifeManager : MonoBehaviour
{
    [SerializeField] Image imageBar;

    private Health health;

    private void Start()
    {
           health = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Health>();
           if (health != null)
            {
                health.HealthChanged += OnHealthChanged;
                OnHealthChanged(health); // Initialize the life bar immediately
            }
           
    }

    public void OnHealthChanged(Health life)
    {
        // Animate the life bar fill amount
        if (imageBar != null)
        {
            imageBar.DOFillAmount(life.NormalizedHealth, 0.5f).SetEase(Ease.OutCubic);
        }
    }

    private void OnDestroy()
    {
        // Clean up any active tweens to prevent memory leaks
        DOTween.Kill(this);
        if (health != null)
        {
            health.HealthChanged -= OnHealthChanged;
            OnHealthChanged(health); // Initialize the life bar immediately
        }
    }
}
