using UnityEngine;
using TMPro;

public class QuestUIManager : MonoBehaviour
{
    [Header("Active Quest")]
    public GameObject activeQuestPanel;
    public TextMeshProUGUI questNameText;
    public TextMeshProUGUI progressText;
    public TextMeshProUGUI timerText;

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

    private void HandleQuestActivated(QuestState quest)
    {
        questNameText.text = quest.sourceQuest.taskID;
        SetProgress(quest);
    }

    private void HandleProgressUpdated(QuestState quest)
    {
        SetProgress(quest);
    }

    private void HandleQuestCompleted(QuestState quest)
    {
        
        completedText.text += $"<color=green>✓  {quest.sourceQuest.taskID}</color>\n";
    }

    private void HandleQuestFailed(QuestState quest)
    {
        activeQuestPanel.SetActive(false);
        completedText.text += $"<color=red>✗  {quest.sourceQuest.taskID}  (timed out)</color>\n";
    }

    // Helpers

    private void SetProgress(QuestState quest)
    {
        progressText.text = $"{quest.currentAmount} / {quest.sourceQuest.requiredCount}";
    }
}
    