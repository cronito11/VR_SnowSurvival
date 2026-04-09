using UnityEngine;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public ItemDefinition itemType;
    public GameObject itemPrefab;
    [Min(1)] public int targetCount = 1;

    private readonly List<GameObject> _spawnedItems = new List<GameObject>();
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
        if (IsRelevantQuest(QuestManager.Instance?.currentActiveQuest))
        {
            SetQuestActive(true);
        }
    }

    private void Update()
    {
        if (_questActive) MaintainTargetCount();
    }

    private void HandleQuestStarted(QuestState quest)
    {
        if (!IsRelevantQuest(quest)) return;
        SetQuestActive(true);
    }

    private void HandleQuestCompleted(QuestState quest)
    {
        if (!IsRelevantQuest(quest)) return;
        SetQuestActive(false);
    }

    private void HandleQuestFailed(QuestState quest)
    {
        if (!IsRelevantQuest(quest)) return;
        SetQuestActive(false);
    }

    private bool IsRelevantQuest(QuestState quest)
    {
        return quest != null && quest.sourceQuest != null && quest.sourceQuest.requiredItem == itemType;
    }

    private void SetQuestActive(bool active)
    {
        _questActive = active;

        if (_questActive)
        {
            MaintainTargetCount();
        }
        else
        {
            ClearSpawnedItems();
        }
    }

    private void MaintainTargetCount()
    {
        _spawnedItems.RemoveAll(item => item == null);

        int missingCount = Mathf.Max(0, targetCount) - _spawnedItems.Count;
        for (int i = 0; i < missingCount; i++)
        {
            SpawnOne();
        }
    }

    private void SpawnOne()
    {
        if (itemPrefab == null)
        {
            Debug.LogError($"[ItemSpawner] itemPrefab not assigned on '{gameObject.name}'!");
            return;
        }

        GameObject spawnedItem = Instantiate(itemPrefab, transform.position, transform.rotation);
        _spawnedItems.Add(spawnedItem);

        Debug.Log($"[ItemSpawner] Spawned '{itemType.displayName}' at '{gameObject.name}'");
    }

    private void ClearSpawnedItems()
    {
        foreach (GameObject item in _spawnedItems)
        {
            if (item != null)
                Destroy(item);
        }

        _spawnedItems.Clear();
    }
}