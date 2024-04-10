using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeathAreaCounter : MonoBehaviour
{
    public Rect areaRect; // Rectangle area for the death zone (in world space)
    public int areaNumber; // Number to identify the area
    private int deathCounter; // Counter for deaths in this area
    public Animator otherAnimator; // Reference to the animator of another gameObject
    public float delayBeforeNextScene = 2.0f; // Duration to wait before loading next scene

    void Start()
    {
        deathCounter = 0; // Initialize the death counter
    }

    void OnDrawGizmosSelected()
    {
        // Draw a wireframe rectangle in the scene view to visualize the death area
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(areaRect.center, areaRect.size);
    }

    // Method to increase the death counter
    public void IncreaseDeathCounter()
    {
        // Update the areaNumber and deathCounter in the DialogueTrigger gameObject
        DialogueTrigger dialogueTrigger = FindDialogueTrigger();
        if (dialogueTrigger != null)
        {
            deathCounter = dialogueTrigger.deathCounter;
            deathCounter++;

            dialogueTrigger.areaNumber = areaNumber;
            dialogueTrigger.deathCounter = deathCounter;

            // Activate the animator and wait before loading next scene
            StartCoroutine(LoadNextSceneWithDelay());
        }
    }

    private DialogueTrigger FindDialogueTrigger()
    {
        // Search for the DialogueTrigger component in the scene
        DialogueTrigger[] dialogueTriggers = FindObjectsOfType<DialogueTrigger>();
        foreach (DialogueTrigger trigger in dialogueTriggers)
        {
            // You can add additional conditions here if needed
            return trigger;
        }
        return null; // DialogueTrigger not found
    }

    private IEnumerator LoadNextSceneWithDelay()
    {
        // Activate the animator
        if (otherAnimator != null)
        {
            otherAnimator.enabled = true;
        }

        // Wait for the specified duration
        yield return new WaitForSeconds(delayBeforeNextScene);

        // Load the next scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
