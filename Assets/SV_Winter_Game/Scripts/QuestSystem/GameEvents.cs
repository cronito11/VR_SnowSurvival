using System;

public static class GameEvents
{
    // Fired by QuestZone when a CollectableItem enters it
    public static event Action<string, string> OnItemDeliveredToZone;
    public static void ItemDeliveredToZone(string itemID, string zoneID) => OnItemDeliveredToZone?.Invoke(itemID, zoneID);

    // Fired when a quest is pulled from Inactive and started
    public static event Action<QuestState> OnQuestActivated;
    public static void QuestActivated(QuestState quest) => OnQuestActivated?.Invoke(quest);

    // Fired when an item is delivered and the UI needs to update its progress counter
    public static event Action<QuestState> OnQuestProgressUpdated;
    public static void QuestProgressUpdated(QuestState quest) => OnQuestProgressUpdated?.Invoke(quest);

    // Fired when a quest reaches its required item count
    public static event Action<QuestState> OnQuestCompleted;
    public static void QuestCompleted(QuestState quest) => OnQuestCompleted?.Invoke(quest);

    // Fired if the timer hits 0 before the quest is completed
    public static event Action<QuestState> OnQuestFailed;
    public static void QuestFailed(QuestState quest) => OnQuestFailed?.Invoke(quest);

    // Fired when every quest in the list has been completed — triggers end-of-game
    public static event Action OnAllQuestsCompleted;
    public static void AllQuestsCompleted() => OnAllQuestsCompleted?.Invoke();
}