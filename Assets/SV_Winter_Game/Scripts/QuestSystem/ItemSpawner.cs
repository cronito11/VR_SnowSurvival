using UnityEngine;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{
    [Header("What to Spawn")]
    [Tooltip("Drag the ItemDefinition SO here. The prefab must have a CollectableItem component.")]
    public ItemDefinition itemType;
    public GameObject itemPrefab;

    [Header("Where & How Many")]
    [Tooltip("The zone ID this spawner belongs to (must match QuestData.fixedZoneID).")]
    public string targetZoneID = "Zone_1";

    [Tooltip("Number of items to keep in the world at all times while the quest is active.")]
    [Min(1)] public int spawnCount = 3;

    [Tooltip("Place empty GameObjects as children to mark spawn positions. " +
             "If left empty the spawner uses its own transform.")]
    public Transform[] spawnPoints;

    private readonly List<GameObject> _spawnedItems = new List<GameObject>();
    private bool _questActive;

    // ── Event subscriptions ─────────────────────────────────
    private void OnEnable()
    {
        GameEvents.OnQuestActivated += HandleQuestStarted;
        GameEvents.OnQuestCompleted += HandleQuestEnded;
        GameEvents.OnQuestFailed    += HandleQuestEnded;
    }

    private void OnDisable()
    {
        GameEvents.OnQuestActivated -= HandleQuestStarted;
        GameEvents.OnQuestCompleted -= HandleQuestEnded;
        GameEvents.OnQuestFailed    -= HandleQuestEnded;
    }

    private void Start()
    {
        // If a quest is already running when the scene loads, catch up.
        if (IsRelevantQuest(QuestManager.Instance?.currentActiveQuest))
            SetQuestActive(true);
    }

    private void Update()
    {
        // Every frame, check if any items were destroyed and refill to spawnCount.
        if (_questActive)
            MaintainCount();
    }

    // ── Quest event handlers ────────────────────────────────

    private void HandleQuestStarted(QuestState quest)
    {
        if (!IsRelevantQuest(quest)) return;
        SetQuestActive(true);
    }

    private void HandleQuestEnded(QuestState quest)
    {
        if (!IsRelevantQuest(quest)) return;
        SetQuestActive(false);
    }

    /// <summary>Turns spawning on or off. Clears all items when deactivated.</summary>
    private void SetQuestActive(bool active)
    {
        _questActive = active;
        if (_questActive)
            MaintainCount();
        else
            ClearSpawnedItems();
    }

    // Helpers 

    /// Returns true if this quest needs OUR item type in OUR zone.
    private bool IsRelevantQuest(QuestState quest)
    {
        if (quest == null || quest.sourceQuest == null) return false;
        return quest.sourceQuest.requiredItem == itemType
            && quest.sourceQuest.fixedZoneID  == targetZoneID;
    }


    // Removes destroyed entries from the list, then spawns replacements
    // so there are always exactly 'spawnCount' items in the world.

    private void MaintainCount()
    {
        if (itemPrefab == null)
        {
            Debug.LogError($"[ItemSpawner] itemPrefab not assigned on '{gameObject.name}'!");
            return;
        }

        // Remove references to items that were collected (destroyed).
        _spawnedItems.RemoveAll(item => item == null);

        // Spawn replacements at the next available spawn points.
        int missing = spawnCount - _spawnedItems.Count;
        for (int i = 0; i < missing; i++)
        {
            Transform point = GetSpawnPoint(_spawnedItems.Count + i);
            GameObject go = Instantiate(itemPrefab, point.position, point.rotation);
            _spawnedItems.Add(go);
        }
    }

    /// Picks a spawn point by index, cycling through the array
    private Transform GetSpawnPoint(int index)
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
            return transform; // fallback: spawner's own position

        return spawnPoints[index % spawnPoints.Length];
    }

    /// Destroys all spawned items and clears the tracking list.
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