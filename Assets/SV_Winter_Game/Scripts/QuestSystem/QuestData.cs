using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "WinterSurvival/Quest Data")]
public class QuestData : ScriptableObject
{
    [Header("Identity")]
    [Tooltip("Unique ID for this task")]
    public string taskID;

    [Header("Requirement")]
    [Tooltip("Drag the ItemDefinition asset here")]
    public ItemDefinition requiredItem;

    [Tooltip("Which zone must the item be dropped into? :QuestZone's zoneID")]
    public string targetZoneID;

    [Tooltip("How many of that item are needed to complete this task.")]
    public int requiredCount = 1;

    [Header("Display")]
    [Tooltip("Short description shown in quest UI. ")]
    [TextArea(1, 3)]
    public string description;
}
