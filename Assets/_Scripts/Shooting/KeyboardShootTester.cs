using UnityEngine;

[DisallowMultipleComponent]
public class KeyboardShootTester : MonoBehaviour
{
    [SerializeField] private SimpleShooter shooter;
    [SerializeField] private KeyCode shootKey = KeyCode.Space;

    private void Reset()
    {
        shooter = GetComponent<SimpleShooter>();
    }

    private void Update()
    {
        if (shooter == null) return;

        if (Input.GetKeyDown(shootKey)) shooter.TriggerDown(); // press once. [web:86]
        if (Input.GetKeyUp(shootKey)) shooter.TriggerUp();     // release. [web:86]
    }
}
