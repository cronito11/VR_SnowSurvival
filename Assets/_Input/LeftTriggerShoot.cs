using UnityEngine;
using UnityEngine.InputSystem;

// NOTE: The type 'ControllerInputActionManager' does not exist in 'UnityEngine.XR.Interaction.Toolkit.Inputs'.
// You need to use the correct type that manages your input actions.
// If you are using the XR Interaction Toolkit, the common type is 'XRInteractionManager' or you may have a custom manager.
// For this example, let's assume you have an 'InputActionAsset' assigned via the inspector.
[RequireComponent(typeof(SimpleShooter))]

public class LeftTriggerShoot : MonoBehaviour
{
    [SerializeField] private InputActionReference triggerAction;
    // Assign: XRI Default Input Actions → XRI LeftHand → Activate

    private SimpleShooter shooter;

    private void Awake()
    {
        shooter = GetComponent<SimpleShooter>();
    }

    private void OnEnable()
    {
        if (triggerAction != null)
        {
            triggerAction.action.performed += OnTriggerDown;
            triggerAction.action.canceled += OnTriggerUp;
            triggerAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (triggerAction != null)
        {
            triggerAction.action.performed -= OnTriggerDown;
            triggerAction.action.canceled -= OnTriggerUp;
            triggerAction.action.Disable();
        }
    }

    private void OnTriggerDown(InputAction.CallbackContext ctx)
    {
        shooter.TriggerDown();
    }

    private void OnTriggerUp(InputAction.CallbackContext ctx)
    {
        shooter.TriggerUp();
    }
}
