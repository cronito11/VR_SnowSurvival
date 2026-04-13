using UnityEngine;
[CreateAssetMenu(fileName = "NewItem", menuName = "WinterSurvival/Item Definition")]
public class ItemDefinition : ScriptableObject
{
    // Unique ID used to match deliveries. Derived from the asset file name.</summary>
    public string ItemID      => name;

    // Name shown in the UI. Derived from the asset file name.</summary>
    public string DisplayName => name;
}
