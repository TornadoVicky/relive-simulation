using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuButtons : MonoBehaviour
{
    public Animator animator; // Reference to the Animator
    public float delayBeforeTransition = 2.0f; // Delay before transitioning to the scene
    public int sceneIndexToLoad = 1; // Scene index to load

    private Button button;

    void Start()
    {
        // Get the button component from the GameObject
        button = GetComponent<Button>();

        // Add an onClick listener to the button
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
        else
        {
            Debug.LogWarning("Button component not found!");
        }
    }

    void OnButtonClick()
    {
        // Enable the animator
        if (animator != null)
        {
            animator.enabled = true;
        }

        // Start a coroutine to delay the scene transition
        StartCoroutine(LoadSceneAfterDelay());
    }

    IEnumerator LoadSceneAfterDelay()
    {
        // Wait for the specified delay before loading the scene
        yield return new WaitForSeconds(delayBeforeTransition);

        RemoveExistingDialogueTriggers();

        // Load the specified scene by index
        SceneManager.LoadScene(sceneIndexToLoad);
    }

    void RemoveExistingDialogueTriggers()
    {
        // Find all DialogueTrigger objects in the scene
        DialogueTrigger[] dialogueTriggers = FindObjectsOfType<DialogueTrigger>();

        // Destroy each DialogueTrigger object found
        foreach (DialogueTrigger dt in dialogueTriggers)
        {
            Destroy(dt.gameObject);
        }
    }
}
