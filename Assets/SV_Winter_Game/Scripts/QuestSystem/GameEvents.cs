using System;

public static class GameEvents
{
    
    // Fired by QuestZone when a CollectableItem enters it
    public static event Action<string, string> OnItemDeliveredToZone;
    public static void ItemDeliveredToZone(string itemID, string zoneID) => OnItemDeliveredToZone?.Invoke(itemID, zoneID);

    // UI & Quest System
    
    // Fired when a quest is pulled from Inactive, given a random zone, and started
    public static event Action<QuestState> OnQuestActivated;
    public static void QuestActivated(QuestState quest) => OnQuestActivated?.Invoke(quest);

    // Fired when an item is delivered and the UI needs to update its 1/3 counter
    public static event Action<QuestState> OnQuestProgressUpdated;
    public static void QuestProgressUpdated(QuestState quest) => OnQuestProgressUpdated?.Invoke(quest);

    // Fired when a quest hits its goal
    public static event Action<QuestState> OnQuestCompleted;
    public static void QuestCompleted(QuestState quest) => OnQuestCompleted?.Invoke(quest);

    // Fired if the timer hits 0 before completion
    public static event Action<QuestState> OnQuestFailed;
    public static void QuestFailed(QuestState quest) => OnQuestFailed?.Invoke(quest);
}