using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    [Header("Master Data")]
    public List<QuestData> allTasks = new List<QuestData>();
    public List<string> allPossibleZones = new List<string>();

    [Header("Live Data (UI Programmer reads this)")]
    public QuestState currentActiveQuest; 
    public List<QuestData> inactiveQuests = new List<QuestData>();
    public List<QuestData> completedQuests = new List<QuestData>();

    private void Awake()
    {
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
        // Handle Timer for the active quest
        if (currentActiveQuest != null)
        {
            currentActiveQuest.timeRemaining -= Time.deltaTime;

            if (currentActiveQuest.timeRemaining <= 0)
            {
                FailQuest();
            }
        }
    }

    // The Zone calls this to check if an item is allowed
    public bool TryDeliverItem(string itemID, string zoneID)
    {
        if (currentActiveQuest == null) 
        {
            Debug.LogWarning("[QuestManager] Delivery Rejected: There is no active quest right now!");
            return false;
        }

        string requiredItemID = currentActiveQuest.sourceQuest.requiredItem.itemID;
        string assignedZone = currentActiveQuest.assignedZoneID;

        // --- DIAGNOSTIC LOG ---
        Debug.Log($"[QuestManager] Delivery Check! \n" +
                  $"You Delivered : Item '{itemID}' to Zone '{zoneID}' \n" +
                  $"Quest Needs   : Item '{requiredItemID}' at Zone '{assignedZone}'");

        // Must match BOTH the required item AND the randomly assigned zone
        if (requiredItemID == itemID && assignedZone == zoneID)
        {
            currentActiveQuest.currentAmount++;
            GameEvents.QuestProgressUpdated(currentActiveQuest); // Tell UI to update numbers
            
            Debug.Log($"[QuestManager] ITEM ACCEPTED! Progress: {currentActiveQuest.currentAmount}/{currentActiveQuest.sourceQuest.requiredCount}");

            if (currentActiveQuest.currentAmount >= currentActiveQuest.sourceQuest.requiredCount)
            {
                CompleteQuest();
            }
            return true; // Item accepted! Zone will destroy it.
        }

        Debug.LogWarning("[QuestManager] ITEM REJECTED: Mismatch found.");
        return false; // Item rejected!
    }

    private void CompleteQuest()
    {
        GameEvents.QuestCompleted(currentActiveQuest); 
        
        // Add to the completed list
        completedQuests.Add(currentActiveQuest.sourceQuest);

        // Loop it back into inactive so the game never runs out of tasks
        // inactiveQuests.Add(currentActiveQuest.sourceQuest); 
        
        currentActiveQuest = null;
        
        // Pull the next one
        ActivateNextQuest();
    }

    private void FailQuest()
    {
        GameEvents.QuestFailed(currentActiveQuest); 
        
        // Loop it back into inactive
        inactiveQuests.Add(currentActiveQuest.sourceQuest); 
        currentActiveQuest = null;
        
        // Pull the next one
        ActivateNextQuest();
    }

    private void ActivateNextQuest()
    {
        if (inactiveQuests.Count == 0) return;

        // Guard
        if (allPossibleZones.Count == 0)
        {
            Debug.LogError("[QuestManager] allPossibleZones is empty! Add zone IDs in the Inspector.");
            return;
        }

        QuestData blueprint = inactiveQuests[0];
        inactiveQuests.RemoveAt(0);

        string randomZone = allPossibleZones[Random.Range(0, allPossibleZones.Count)];
        currentActiveQuest = new QuestState(blueprint, randomZone, 0);
        GameEvents.QuestActivated(currentActiveQuest);
    }
}