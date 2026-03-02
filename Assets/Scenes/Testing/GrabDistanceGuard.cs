using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GrabDistanceGuard : MonoBehaviour
{
    [Header("Distance Limits")]
    public float minDistance = 0.25f; 
    public float maxDistance = 2.5f;

    private XRGrabInteractable _grabInteractable;

    void Awake()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void Update()
    {
        // Only run this logic if the object is actually being held
        if (_grabInteractable.isSelected)
        {
            // Get the position of the hand (interactor) holding the object
            Vector3 handPos = _grabInteractable.interactorsSelecting[0].transform.position;
            Vector3 objectPos = transform.position;

            float currentDist = Vector3.Distance(handPos, objectPos);

            if (currentDist > maxDistance || currentDist < minDistance)
            {
                // Calculate where the object SHOULD be
                float clampedDist = Mathf.Clamp(currentDist, minDistance, maxDistance);
                Vector3 direction = (objectPos - handPos).normalized;
                
                // Force the object to stay within the limit
                transform.position = handPos + (direction * clampedDist);
            }
        }
    }
}