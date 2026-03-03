using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public ItemDefinition itemType;
    public GameObject itemPrefab;

    private GameObject _currentSpawnedItem;

    private void OnEnable()
    {
        GameEvents.OnQuestActivated       += HandleQuestStarted;
        GameEvents.OnQuestProgressUpdated += HandleItemDelivered;
        GameEvents.OnQuestCompleted       += HandleQuestCompleted;
        GameEvents.OnQuestFailed          += HandleQuestFailed;
    }

    private void OnDisable()
    {
        GameEvents.OnQuestActivated       -= HandleQuestStarted;
        GameEvents.OnQuestProgressUpdated -= HandleItemDelivered;
        GameEvents.OnQuestCompleted       -= HandleQuestCompleted;
        GameEvents.OnQuestFailed          -= HandleQuestFailed;
    }

    private void Start()
    {
        // FIX: If QuestManager already fired OnQuestActivated before
        // this spawner subscribed, catch it here and spawn immediately
        if (QuestManager.Instance?.currentActiveQuest != null)
        {
                SpawnItem();
        }
    }

    private void HandleQuestStarted(QuestState quest)
    {
        if (quest.sourceQuest.requiredItem != itemType) return;
        SpawnItem();
    }

    private void HandleItemDelivered(QuestState quest)
    {
        if (quest.sourceQuest.requiredItem != itemType) return;
        SpawnItem(); // respawn every delivery, no limit
    }

    private void HandleQuestCompleted(QuestState quest)
    {
        if (quest.sourceQuest.requiredItem != itemType) return;
        if (_currentSpawnedItem != null)
            Destroy(_currentSpawnedItem);
    }

    private void HandleQuestFailed(QuestState quest)
    {
        if (quest.sourceQuest.requiredItem != itemType) return;
        if (_currentSpawnedItem != null)
            Destroy(_currentSpawnedItem);
    }

    private void SpawnItem()
    {
        if (itemPrefab == null)
        {
            Debug.LogError($"[ItemSpawner] itemPrefab not assigned on '{gameObject.name}'!");
            return;
        }

        if (_currentSpawnedItem != null)
            Destroy(_currentSpawnedItem);

        _currentSpawnedItem = Instantiate(itemPrefab, transform.position, transform.rotation);
        Debug.Log($"[ItemSpawner] Spawned '{itemType.displayName}' at '{gameObject.name}'");
    }
}