using UnityEngine;

public class ZoneText : MonoBehaviour
{
    private QuestZone questZone;
    private TMPro.TextMeshProUGUI textMesh;

    private void Start()
    {
        questZone = GetComponentInParent<QuestZone>();
        if (questZone == null)
        {
            Debug.LogError("ZoneText: No QuestZone found in parent hierarchy.");
            return;
        }

        textMesh = GetComponent<TMPro.TextMeshProUGUI>();
        textMesh.text = questZone.zoneID;
    }
}
