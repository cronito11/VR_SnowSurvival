// Global Events for the game
using System;

public class GameEvents
{
        // Fired by QuestZone when a CollectableItem enters it
        public static event Action<string, string> OnItemDeliveredToZone;
        
        // Fired by QuestManager when a task's counter is fully met
        public static event Action<string> OnTaskCompleted;
        
        // Fired when all tasks are completed.
        public static event Action OnAllTasksCompleted;
        
        // Invoke methods
        public static void ItemDeliveredToZone(string itemID, string zoneID)
                => OnItemDeliveredToZone?.Invoke(itemID, zoneID);

        public static void TaskCompleted(string taskID)
                => OnTaskCompleted?.Invoke(taskID);
        
        public static void AllTasksCompleted()
                => OnAllTasksCompleted?.Invoke();
}
