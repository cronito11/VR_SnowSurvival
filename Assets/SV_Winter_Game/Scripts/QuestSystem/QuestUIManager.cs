using UnityEngine;
using TMPro;

public class QuestUIManager : MonoBehaviour
{
    [Header("Active Quest")]
    public TextMeshProUGUI questNameText;
    public TextMeshProUGUI progressText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI zone;

    [Header("Completed Quests")]
    public TextMeshProUGUI completedText; 
    
    [Header("Failed Quests")]
    public TextMeshProUGUI failedText;

    private void Awake()
    {
        completedText.text = "";  
        failedText.text = "";
    }

    private void OnEnable()
    {
        GameEvents.OnQuestActivated       += HandleQuestActivated;
        GameEvents.OnQuestProgressUpdated += HandleProgressUpdated;
        GameEvents.OnQuestCompleted       += HandleQuestCompleted;
        GameEvents.OnQuestFailed          += HandleQuestFailed;
    }

    private void OnDisable()
    {
        GameEvents.OnQuestActivated       -= HandleQuestActivated;
        GameEvents.OnQuestProgressUpdated -= HandleProgressUpdated;
        GameEvents.OnQuestCompleted       -= HandleQuestCompleted;
        GameEvents.OnQuestFailed          -= HandleQuestFailed;
    }

    private void Update()
    {
        if (QuestManager.Instance?.currentActiveQuest == null)
        {
            timerText.text = "";
            return;
        }

        float time = QuestManager.Instance.currentActiveQuest.timeRemaining;
        timerText.text  = Mathf.Ceil(time) + "s";
        timerText.color = time <= 10f ? Color.red : Color.white;
    }

    // Handlers 

    // <Shows the new quest name, zone, and resets the progress counter.
    private void HandleQuestActivated(QuestState quest)
    {
        questNameText.text = quest.sourceQuest.TaskID;
        zone.text = $"Zone: {quest.assignedZoneID}";
        SetProgress(quest);
    }

    // Updates the "1 / 3" progress text.
    private void HandleProgressUpdated(QuestState quest)
    {
        SetProgress(quest);
    }

    // Appends a green checkmark entry to the completed list.
    private void HandleQuestCompleted(QuestState quest)
    {
        completedText.text += $"<color=green>✓  {quest.sourceQuest.TaskID}</color>\n";
    }

    // Appends a red cross entry to the completed list (timed out).
    private void HandleQuestFailed(QuestState quest)
    {
        completedText.text += $"<color=red>✗  {quest.sourceQuest.TaskID}  (timed out)</color>\n";
    }

    // Helpers

    private void SetProgress(QuestState quest)
    {
        progressText.text = $"{quest.currentAmount} / {quest.sourceQuest.requiredCount}";
    }
}
    