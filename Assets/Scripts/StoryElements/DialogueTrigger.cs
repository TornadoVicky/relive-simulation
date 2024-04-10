using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueTrigger : MonoBehaviour
{
    public int areaNumber;
    public int deathCounter;
    private static DialogueTrigger instance;
    private DialogueHandler dialogueHandler;
    public bool isShowingDialogue = false;

    private void Awake()
    {
        // Singleton pattern: check if an instance already exists
        if (instance != null && instance != this)
        {
            // Destroy the new instance to maintain a single instance
            Destroy(gameObject);
            return;
        }

        // Set the current instance as the singleton instance
        instance = this;

        // Don't destroy this object when loading a new scene
        DontDestroyOnLoad(gameObject);

        // Find the DialogueHandler script in the scene
        dialogueHandler = FindObjectOfType<DialogueHandler>();

        // If DialogueHandler is not found in the current scene, check if it's available in other scenes
        if (dialogueHandler == null)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            // If DialogueHandler is found, start listening for input
            StartListeningForInput();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find the DialogueHandler script in the scene
        dialogueHandler = FindObjectOfType<DialogueHandler>();

        // If DialogueHandler is found, start listening for input
        if (dialogueHandler != null)
        {
            StartListeningForInput();
        }
    }

    private void StartListeningForInput()
    {
        // Start listening for input
        enabled = true;
    }

    private void Update()
    {
        // For demonstration purposes, you can trigger the dialogue display with a key press
        if (Input.GetKeyDown(KeyCode.Return) && !isShowingDialogue)
        {
            TriggerDialogue();
            isShowingDialogue = true;
        }
    }

    private void TriggerDialogue()
    {
        if (dialogueHandler != null)
        {
            dialogueHandler.ShowDialogueSet(areaNumber, deathCounter);
        }
    }
}
