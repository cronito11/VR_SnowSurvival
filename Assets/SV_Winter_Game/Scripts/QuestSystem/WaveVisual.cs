using UnityEngine;

public class WaveVisual : MonoBehaviour
{
    public float pulseSpeed = 2.0f;
    public float maxScale = 1.5f;
    private Vector3 _baseScale;
    private MeshRenderer _renderer;

    void Awake() {
        _baseScale = transform.localScale;
        _renderer = GetComponent<MeshRenderer>();
    }

    void Update() {
        // Calculate a 0 to 1 value based on time
        float wave = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
        
        // Scale the wave up and down
        transform.localScale = _baseScale * (1 + (wave * 0.2f));
        
        // Change transparency (Alpha) based on the wave
        Color c = _renderer.material.color;
        c.a = wave * 0.5f; // Keep it semi-transparent
        _renderer.material.color = c;
    }
}