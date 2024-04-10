using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadMainLevel : MonoBehaviour
{
    public Animator otherAnimator;
    public float delayBeforeNextScene = 2.4f;
    public AudioSource TeleportAudioSource;

    // Reference to the 2D collider
    private Collider2D myCollider;
    private bool hasLoadedScene = false; // Flag to prevent multiple scene loads

    void Start()
    {
        // Get the reference to the 2D collider component
        myCollider = GetComponent<Collider2D>();

        // Make sure the collider is not null
        if (myCollider == null)
        {
            Debug.LogError("Collider2D component not found!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the other collider is on the "Player" layer and scene is not already loaded
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !hasLoadedScene)
        {
            TeleportAudioSource.Play();
            StartCoroutine(LoadMainScene());
            hasLoadedScene = true; // Set flag to true to prevent multiple scene loads
        }
    }

    private IEnumerator LoadMainScene()
    {
        // Activate the animator
        if (otherAnimator != null)
        {
            otherAnimator.enabled = true;
        }

        // Wait for the specified duration
        yield return new WaitForSeconds(delayBeforeNextScene);
        TeleportAudioSource.Stop();

        // Load scene number 0
        SceneManager.LoadScene(1);
    }
}
