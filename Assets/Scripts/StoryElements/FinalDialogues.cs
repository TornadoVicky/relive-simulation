using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class FinalDialogues : MonoBehaviour
{
    public Collider2D triggerCollider; // Reference to the Collider2D
    public Animator dialoguesAnimator; // Reference to the Animator
    public Animator endAnimator;
    public TextMeshProUGUI dialoguesText; // Reference to the TextMeshProUGUI component
    public float dialogueInterval = 2.0f; // Interval between dialogues
    public float portalActivationDelay = 5.0f;
    public GameObject portalObject;

    public List<string> dialogues = new List<string>(); // List of dialogues
    public string endDialogue; // End dialogue when health is low

    private bool dialoguesStarted = false; // Flag to track if dialogues have started
    private Coroutine dialogueCoroutine; // Coroutine reference for showing dialogues
    public PlayerHealth playerHealth; // Reference to the PlayerHealth component
    public AudioSource audioSource; // Reference to the AudioSource component

    private bool hasStartedDialogue = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasStartedDialogue)
        {   
            StartCoroutine(ActivatePortalAfterDelay());
            StartDialogues();
            hasStartedDialogue = true;

            // Play audio source
            if (audioSource != null)
            {
                audioSource.Play();
            }
        }
    }

    private void Update()
    {
        DeathSequence();
    }

    void StartDialogues()
    {
        if (dialoguesAnimator != null && dialoguesText != null && dialogues.Count > 0)
        {
            dialoguesAnimator.enabled = true; // Enable the animator
            dialoguesStarted = true; // Set flag to true

            // Start showing dialogues
            dialogueCoroutine = StartCoroutine(ShowDialogues());
        }
    }

    void StopDialogues()
    {
        if (dialoguesStarted)
        {
            dialoguesStarted = false; // Reset flag
            StopCoroutine(dialogueCoroutine); // Stop the dialogue coroutine

            // Stop audio source
            if (audioSource != null)
            {
                audioSource.Stop();
            }
        }
    }

    IEnumerator ShowDialogues()
    {
        foreach (string dialogue in dialogues)
        {
            dialoguesText.text = dialogue; // Display the dialogue in TMP

            // Wait for the specified interval before showing the next dialogue
            yield return new WaitForSeconds(dialogueInterval);
        }
    }

    void DeathSequence()
    {
        if (playerHealth != null && playerHealth.currentHealth < 49900)
        {
            ShowEndDialogue();
        }
    }

    void ShowEndDialogue()
    {
        if (endAnimator != null && dialoguesText != null && !string.IsNullOrEmpty(endDialogue))
        {
            dialoguesText.text = endDialogue; // Display the end dialogue in TMP
            endAnimator.enabled = true; // Enable the animator

            StartCoroutine(EndGame());
        }
    }

    IEnumerator ActivatePortalAfterDelay()
    {
        yield return new WaitForSeconds(portalActivationDelay);

        if (portalObject != null)
        {
            portalObject.SetActive(true); // Activate the portal object
        }
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(2.4f);

        SceneManager.LoadScene(3);
    }
}
