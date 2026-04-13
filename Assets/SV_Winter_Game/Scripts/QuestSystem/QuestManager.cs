using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    [Header("Master Data")]
    public List<QuestData> allTasks = new List<QuestData>();

    [Header("Live Data (UI Programmer reads this)")]
    public QuestState currentActiveQuest; 
    public List<QuestData> inactiveQuests = new List<QuestData>();
    public List<QuestData> completedQuests = new List<QuestData>();

    private void Awake()
    {
        // Singleton pattern — only one QuestManager allowed.
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        // Remove any accidental empty slots from the Inspector
        allTasks.RemoveAll(task => task == null);
        
        if (allTasks.Count == 0)
        {
            Debug.LogError("[QuestManager] There are NO valid tasks in the All Tasks list!");
            return;
        }
        
        // Put all tasks into the inactive pool to start
        inactiveQuests.AddRange(allTasks);
        
        // Start the very first task
        ActivateNextQuest();
    }

    private void Update()
    {
        // Count down the timer for the active quest. Fail it if time runs out.
        if (currentActiveQuest != null)
        {
            currentActiveQuest.timeRemaining -= Time.deltaTime;

            if (currentActiveQuest.timeRemaining <= 0)
            {
                FailQuest();
            }
        }
    }

    /// Quick check: can this item be delivered to this zone right now?
    public bool IsItemDeliverable(string itemID, string zoneID)
    {
        if (currentActiveQuest == null) return false;
        string requiredItemID = currentActiveQuest.sourceQuest.requiredItem.ItemID;
        string assignedZone = currentActiveQuest.assignedZoneID;
        return requiredItemID == itemID && assignedZone == zoneID;
    }


    // Called by QuestZone when the player delivers an item.
    // Returns true if the item matches the active quest's requirement and zone.
    // Increments progress and completes the quest if the goal is reached.

    public bool TryDeliverItem(string itemID, string zoneID)
    {
        if (currentActiveQuest == null) 
        {
            Debug.LogWarning("[QuestManager] Delivery Rejected: There is no active quest right now!");
            return false;
        }

        string requiredItemID = currentActiveQuest.sourceQuest.requiredItem.ItemID;
        string assignedZone = currentActiveQuest.assignedZoneID;

        Debug.Log($"[QuestManager] Delivery Check! \n" +
                  $"You Delivered : Item '{itemID}' to Zone '{zoneID}' \n" +
                  $"Quest Needs   : Item '{requiredItemID}' at Zone '{assignedZone}'");

        // Must match BOTH the required item AND the quest's fixed zone
        if (requiredItemID == itemID && assignedZone == zoneID)
        {
            currentActiveQuest.currentAmount++;
            GameEvents.QuestProgressUpdated(currentActiveQuest);
            
            Debug.Log($"[QuestManager] ITEM ACCEPTED! Progress: {currentActiveQuest.currentAmount}/{currentActiveQuest.sourceQuest.requiredCount}");

            if (currentActiveQuest.currentAmount >= currentActiveQuest.sourceQuest.requiredCount)
            {
                CompleteQuest();
            }
            return true;
        }

        Debug.LogWarning("[QuestManager] ITEM REJECTED: Mismatch found.");
        return false;
    }
    
    // Marks the current quest as completed and moves to the next one.
    // If no quests remain, fires AllQuestsCompleted to end the game.
    // Completed quests are never put back into the inactive pool.
    private void CompleteQuest()
    {
        GameEvents.QuestCompleted(currentActiveQuest); 
        
        completedQuests.Add(currentActiveQuest.sourceQuest);
        currentActiveQuest = null;
        
        if (inactiveQuests.Count > 0)
        {
            ActivateNextQuest();
        }
        else
        {
            // All quests done — notify the game to end.
            Debug.Log("[QuestManager] ALL QUESTS COMPLETED! Game Over.");
            GameEvents.AllQuestsCompleted();
        }
    }


    // Marks the current quest as failed and puts it back into the inactive pool
    // so the player gets another chance at it later.

    private void FailQuest()
    {
        GameEvents.QuestFailed(currentActiveQuest); 
        
        // Recycle the failed quest so it can be retried.
        inactiveQuests.Add(currentActiveQuest.sourceQuest); 
        currentActiveQuest = null;
        
        ActivateNextQuest();
    }

    /// Pulls the next quest from the inactive pool and activates it.
    private void ActivateNextQuest()
    {
        if (inactiveQuests.Count == 0) return;

        QuestData blueprint = inactiveQuests[0];
        inactiveQuests.RemoveAt(0);

        if (blueprint == null || string.IsNullOrWhiteSpace(blueprint.fixedZoneID))
        {
            Debug.LogError("[QuestManager] Quest is missing a fixedZoneID. Set it in QuestData.");
            return;
        }

        currentActiveQuest = new QuestState(blueprint, blueprint.fixedZoneID, 0);
        GameEvents.QuestActivated(currentActiveQuest);
    }
}