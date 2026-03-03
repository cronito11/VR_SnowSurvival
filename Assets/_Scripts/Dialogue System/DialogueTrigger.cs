using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private DialogueAsset dialogue;

    [Header("Rules")]
    [SerializeField] private bool fireOnce = true;
    [SerializeField] private float cooldownSeconds = 0f;
    [SerializeField] private string requiredTag = "Player";

    [Header("Designer Hooks")]
    public UnityEvent onTriggered; // assign extra actions in Inspector [web:31]

    private bool _hasFired;
    private float _nextAllowedTime;

    public void Trigger()
    {
        if (!CanFire()) return;

        if (dialogueManager != null && dialogue != null)
            dialogueManager.Play(dialogue);

        onTriggered?.Invoke();
        MarkFired();
    }

    private bool CanFire()
    {
        if (fireOnce && _hasFired) return false;
        if (Time.time < _nextAllowedTime) return false;
        return true;
    }

    private void MarkFired()
    {
        _hasFired = true;
        if (cooldownSeconds > 0f) _nextAllowedTime = Time.time + cooldownSeconds;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(requiredTag) && !other.CompareTag(requiredTag)) return;
        Trigger();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!string.IsNullOrEmpty(requiredTag) && !other.CompareTag(requiredTag)) return;
        Trigger();
    }
}
