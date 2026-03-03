using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public ItemDefinition itemType;
    public GameObject itemPrefab;

    private GameObject _currentSpawnedItem;
    private bool _questActive;

    private void OnEnable()
    {
        GameEvents.OnQuestActivated += HandleQuestStarted;
        GameEvents.OnQuestCompleted += HandleQuestCompleted;
        GameEvents.OnQuestFailed    += HandleQuestFailed;
    }

    private void OnDisable()
    {
        GameEvents.OnQuestActivated -= HandleQuestStarted;
        GameEvents.OnQuestCompleted -= HandleQuestCompleted;
        GameEvents.OnQuestFailed    -= HandleQuestFailed;
    }

    private void Start()
    {
        if (QuestManager.Instance?.currentActiveQuest != null &&
            QuestManager.Instance.currentActiveQuest.sourceQuest.requiredItem == itemType)
        {
            _questActive = true;
            SpawnItem();
        }
    }

    private void Update()
    {
        // Auto-respawn logic
        if (_questActive && _currentSpawnedItem == null)
        {
            SpawnItem();
        }
    }

    private void HandleQuestStarted(QuestState quest)
    {
        if (quest.sourceQuest.requiredItem != itemType) return;

        _questActive = true;
        SpawnItem();
    }

    private void HandleQuestCompleted(QuestState quest)
    {
        if (quest.sourceQuest.requiredItem != itemType) return;

        _questActive = false;

        if (_currentSpawnedItem != null)
            Destroy(_currentSpawnedItem);
    }

    private void HandleQuestFailed(QuestState quest)
    {
        if (quest.sourceQuest.requiredItem != itemType) return;

        _questActive = false;

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
            return;

        _currentSpawnedItem = Instantiate(itemPrefab, transform.position, transform.rotation);

        Debug.Log($"[ItemSpawner] Spawned '{itemType.displayName}' at '{gameObject.name}'");
    }
}