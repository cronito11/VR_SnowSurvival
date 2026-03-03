using UnityEngine;

public class BearSfx : MonoBehaviour
{
    [SerializeField] private AudioSource source;

    [Header("Clips")]
    [SerializeField] private AudioClip detectClip;
    [SerializeField] private AudioClip attackClip;
    [SerializeField] private AudioClip deathClip;

    [Header("Volumes")]
    [Range(0f, 1f)][SerializeField] private float detectVol = 1f;
    [Range(0f, 1f)][SerializeField] private float attackVol = 1f;
    [Range(0f, 1f)][SerializeField] private float deathVol = 1f;

    private bool _playedDetect;

    private void Reset() => source = GetComponent<AudioSource>();
    private void Awake() { if (!source) source = GetComponent<AudioSource>(); }

    public void OnDetectPlayer()
    {
        if (_playedDetect) return;
        _playedDetect = true;
        Play(detectClip, detectVol);
        Debug.Log("sfx on player detect");
    }

    public void ResetDetect() => _playedDetect = false;

    public void OnAttack()
    {
        Play(attackClip, attackVol);
        Debug.Log("sfx on player attack");
    }
    public void OnDeath() => Play(deathClip, deathVol);

    private void Play(AudioClip clip, float vol)
    {
        if (!clip || !source) return;
        source.PlayOneShot(clip, vol); // does not cancel other clips playing. [web:138]
    }
}
