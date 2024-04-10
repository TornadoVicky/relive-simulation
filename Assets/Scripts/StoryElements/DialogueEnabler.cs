using UnityEngine;

public class DialogueEnabler : MonoBehaviour
{
    void Start()
    {
        // Find all game objects with the DialogueTrigger script in the scene
        DialogueTrigger[] dialogueTriggers = FindObjectsOfType<DialogueTrigger>();

        // Loop through each DialogueTrigger and set isShowingDialogue to false
        foreach (DialogueTrigger trigger in dialogueTriggers)
        {
            trigger.isShowingDialogue = false;
        }
    }
}
