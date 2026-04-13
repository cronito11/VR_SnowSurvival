using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "WinterSurvival/Quest Data")]
public class QuestData : ScriptableObject
{
    /// Unique task ID shown in the UI. Derived from the asset file name.
    public string TaskID => name;

    [Header("Requirement")]
    [Tooltip("Drag the ItemDefinition asset here")]
    public ItemDefinition requiredItem;

    [Tooltip("How many of that item are needed to complete this task.")]
    public int requiredCount = 1;

    [Tooltip("How many seconds the player has to complete this task.")]
    public float timeLimitInSec = 60f;

    [Header("Zone")]
    [Tooltip("Exact zone ID this task should be delivered to.")]
    public string fixedZoneID = "Zone_1";

    [Header("Display")]
    [Tooltip("Optional description. If empty, one is auto-generated.")]
    [TextArea(1, 3)]
    public string description;


    // Returns the description if provided,
    // otherwise auto-generates one like "Deliver 3x Wood to Zone_1".
    public string Description
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(description)) return description;
            string itemName = requiredItem != null ? requiredItem.DisplayName : "???";
            return $"Deliver {requiredCount}x {itemName} to {fixedZoneID}";
        }
    }
}