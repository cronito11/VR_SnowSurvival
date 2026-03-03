using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_LifeManager : MonoBehaviour
{
    [SerializeField] Image imageBar;

    public void OnLifeChange(float life)
    {
        // Animate the life bar fill amount
        if (imageBar != null)
        {
            imageBar.DOFillAmount(life, 0.5f).SetEase(Ease.OutCubic);
        }
    }

    private void OnDestroy()
    {
        // Clean up any active tweens to prevent memory leaks
        DOTween.Kill(this);
    }
}
