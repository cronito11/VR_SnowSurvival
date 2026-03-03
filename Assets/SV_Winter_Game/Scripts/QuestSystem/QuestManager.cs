using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    [Header("All Tasks In The Game")]
    public List<QuestData> allTasks = new List<QuestData>();

    // Counter for the tasks
    private int _totalTasks;
    private int _completedTasks;

    // set of taskIDs that are done
    private HashSet<string> _completedTaskIDs = new HashSet<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _totalTasks = allTasks.Count;
        _completedTasks = 0;
    }

    private void OnEnable()  => GameEvents.OnTaskCompleted += HandleTaskCompleted;
    private void OnDisable() => GameEvents.OnTaskCompleted -= HandleTaskCompleted;

    private void HandleTaskCompleted(string taskID)
    {
        // Ignore if already marked done
        if (_completedTaskIDs.Contains(taskID)) return;

        _completedTaskIDs.Add(taskID);
        _completedTasks++;

        Debug.Log($"[QuestManager] Tasks done: {_completedTasks}/{_totalTasks}");

        if (_completedTasks >= _totalTasks)
        {
            Debug.Log("[QuestManager] ALL TASKS COMPLETE - GAME DONE!");
            GameEvents.AllTasksCompleted();
        }
    }

    public bool IsTaskComplete(string taskID) => _completedTaskIDs.Contains(taskID);
}