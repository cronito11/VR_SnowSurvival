using UnityEngine;

public class PointeTo : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform targetZone;
    
    [Header("Settings")]
    [Tooltip("If true, finds the active quest zone automatically from QuestManager")]
    [SerializeField] private bool autoFindQuestZone = true;
    
    [Tooltip("If true, only rotates on the Y axis (horizontal rotation)")]
    [SerializeField] private bool lockToYAxis = true;
    
    [Header("Optional Smoothing")]
    [SerializeField] private bool smoothRotation = true;
    [Range(0.1f, 20f)]
    [SerializeField] private float rotationSpeed = 5f;

    private void Start()
    {
        if (autoFindQuestZone)
        {
            GameEvents.OnQuestActivated += OnQuestActivated;
            UpdateTargetZone();
        }
    }

    private void OnDestroy()
    {
        if (autoFindQuestZone)
        {
            GameEvents.OnQuestActivated -= OnQuestActivated;
        }
    }

    private void OnQuestActivated(QuestState quest)
    {
        UpdateTargetZone();
    }

    private void UpdateTargetZone()
    {
        if (QuestManager.Instance == null || QuestManager.Instance.currentActiveQuest == null)
        {
            targetZone = null;
            return;
        }

        string zoneID = QuestManager.Instance.currentActiveQuest.assignedZoneID;
        QuestZone[] zones = Object.FindObjectsByType<QuestZone>(FindObjectsSortMode.None);
        
        foreach (var zone in zones)
        {
            if (zone.zoneID == zoneID)
            {
                targetZone = zone.transform;
                break;
            }
        }
    }

    void Update()
    {
        if (targetZone == null)
        {
            if (autoFindQuestZone)
            {
                UpdateTargetZone();
            }
            return;
        }

        Vector3 direction = targetZone.position - transform.position;
        
        if (direction.sqrMagnitude < 0.001f) return;

        Quaternion targetRotation;
        
        if (lockToYAxis)
        {
            direction.y = 0;
            if (direction.sqrMagnitude < 0.001f) return;
            targetRotation = Quaternion.LookRotation(direction);
            
            Vector3 eulerAngles = targetRotation.eulerAngles;
            eulerAngles.y = 0;
            targetRotation = Quaternion.Euler(eulerAngles);
        }
        else
        {
            targetRotation = Quaternion.LookRotation(direction);
        }

        if (smoothRotation)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        else
        {
            transform.rotation = targetRotation;
        }
        
        Vector3 localRot = transform.localEulerAngles;
        localRot.y = 0;
        transform.localEulerAngles = localRot;
    }
}
