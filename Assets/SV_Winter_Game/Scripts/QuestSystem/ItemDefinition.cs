using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "WinterSurvival/Item Definition")]
public class ItemDefinition : ScriptableObject
{
    [Header("Identity")]
    [Tooltip("Unique ID used by the event")]
    public string itemID;
    
    [Header("Display")]
    [Tooltip("Name shown to the player")]
    public string displayName;
    
    [Tooltip("Icon shown in the UI")]
    public Sprite icon;
}
