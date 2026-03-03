using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Game/Dialogue Asset", order = 0)]
public class DialogueAsset : ScriptableObject
{
    public List<DialogueLine> lines = new();

}
[Serializable]
public struct DialogueLine
{
    [TextArea(2, 5)] public string text;
    [Min(0f)] public float duration;
    public bool waitForInput;
}