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

    [Tooltip("How many of that item are needed to complete this task.")]
    public int requiredCount = 1;

    [Tooltip("How many seconds the player has to complete this task.")]
    public float timeLimitInSec = 60f;

    [Header("Display")]
    [Tooltip("Short description shown in quest UI. ")]
    [TextArea(1, 3)]
    public string description;
}