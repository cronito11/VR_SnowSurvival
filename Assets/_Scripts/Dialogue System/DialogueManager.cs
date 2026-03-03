using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("Singleton")]
    [SerializeField] private bool dontDestroyOnLoad = true;

    [Header("UI")]
    [SerializeField] private GameObject uiRoot;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Behavior")]
    [SerializeField] private bool queueIfBusy = true;
    [SerializeField] private bool useUnscaledTime = false;
    [SerializeField] private KeyCode continueKey = KeyCode.Space;

    private bool _isPlaying;
    private readonly Queue<DialogueAsset> _queue = new();

    public bool IsPlaying => _isPlaying;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (dontDestroyOnLoad)
            DontDestroyOnLoad(gameObject); // works on root objects [web:61]
    }

    public void Play(DialogueAsset dialogue)
    {
        if (dialogue == null || dialogue.lines == null || dialogue.lines.Count == 0) return;

        if (_isPlaying)
        {
            if (queueIfBusy) _queue.Enqueue(dialogue);
            return;
        }

        StartCoroutine(RunDialogue(dialogue));
    }

    public void StopAll()
    {
        StopAllCoroutines();
        _queue.Clear();
        _isPlaying = false;
        if (uiRoot != null) uiRoot.SetActive(false);
    }

    private IEnumerator RunDialogue(DialogueAsset dialogue)
    {
        _isPlaying = true;
        if (uiRoot != null) uiRoot.SetActive(true);

        for (int i = 0; i < dialogue.lines.Count; i++)
        {
            var line = dialogue.lines[i];

            if (dialogueText != null)
                dialogueText.text = line.text;

            if (line.waitForInput)
            {
                yield return new WaitUntil(() => Input.GetKeyDown(continueKey));
            }
            else
            {
                if (line.duration <= 0f) yield return null;
                else if (useUnscaledTime) yield return new WaitForSecondsRealtime(line.duration);
                else yield return new WaitForSeconds(line.duration);
            }
        }

        if (uiRoot != null) uiRoot.SetActive(false);
        _isPlaying = false;

        if (_queue.Count > 0)
            Play(_queue.Dequeue());
    }
}
