using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class DialogueSet
{
    public List<string> dialogues = new List<string>();
}

public class DialogueHandler : MonoBehaviour
{
    [Serializable]
    public struct DialogueKey
    {
        public int areaNumber;
        public int deathCounterStart;
        public int deathCounterEnd;
        public DialogueSet dialogueSet; // Assign the corresponding dialogue set in the Inspector
    }

    public DialogueKey[] dialogueKeys;
    public float dialogueGap = 1.0f; // Gap between dialogues
    public TextMeshProUGUI dialogueText; // Reference to TextMeshPro component
    public AudioSource dialogueAudioSource; // Reference to the AudioSource component

    private Coroutine dialogueCoroutine; // Coroutine reference for showing dialogues

    private void Start()
    {

    }

    public void ShowDialogueSet(int areaNumber, int deathCounter)
    {
        foreach (DialogueKey key in dialogueKeys)
        {
            if (key.areaNumber == areaNumber && deathCounter >= key.deathCounterStart && deathCounter <= key.deathCounterEnd)
            {
                dialogueCoroutine = StartCoroutine(DisplayDialogues(key.dialogueSet.dialogues));
                return; // Exit the method after finding the matching key
            }
        }
    }

    private IEnumerator DisplayDialogues(List<string> dialogues)
    {
        // Play audio while dialogues are being displayed
        if (dialogueAudioSource != null && !dialogueAudioSource.isPlaying)
        {
            dialogueAudioSource.Play();
        }

        foreach (string dialogue in dialogues)
        {
            // Display dialogue in TextMeshPro component
            dialogueText.text = dialogue;
            yield return new WaitForSeconds(dialogueGap);
        }

        // Stop playing audio after dialogues end
        if (dialogueAudioSource != null && dialogueAudioSource.isPlaying)
        {
            dialogueAudioSource.Stop();
        }
    }

    private void OnDestroy()
    {
        // Ensure audio source is stopped when the script is destroyed
        if (dialogueAudioSource != null && dialogueAudioSource.isPlaying)
        {
            dialogueAudioSource.Stop();
        }
    }
}
