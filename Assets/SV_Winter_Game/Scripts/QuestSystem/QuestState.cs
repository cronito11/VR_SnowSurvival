using System;
using UnityEngine;

[Serializable] 
public class QuestState
{
    public QuestData sourceQuest; 
    public string assignedZoneID; 
    public int currentAmount;     
    public float timeRemaining;   

    public QuestState(QuestData data, string zone, int currentAmt)
    {
        if (data == null)
        {
            Debug.LogError("[QuestState] CRITICAL: Tried to start a quest, but the QuestData was missing!");
            return;
        }

        sourceQuest = data;
        assignedZoneID = zone;
        currentAmount = currentAmt;
        timeRemaining = data.timeLimitInSec;
    }
}