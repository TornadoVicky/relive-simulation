using UnityEngine;

public class FireDamage : MonoBehaviour
{
    public int damageAmount = 10;
    public float damageInterval = 1f; // Time interval between damage ticks

    private float timer;
    private PlayerHealth playerHealth;

    void Start()
    {
        // Find the PlayerHealth component in the parent GameObject
        playerHealth = GetComponentInParent<PlayerHealth>();
        if (playerHealth == null || !playerHealth.enabled)
        {
            enabled = false; // Disable this script if PlayerHealth is not found or not enabled
        }
    }

    void Update()
    {
        // Update the timer
        timer += Time.deltaTime;

        // Check if it's time to apply damage based on the damage interval
        if (timer >= damageInterval)
        {
            timer = 0f; // Reset the timer
            ApplyDamage();
        }
    }

    void ApplyDamage()
    {
        // Check if PlayerHealth component is available, valid, and enabled
        if (playerHealth != null && playerHealth.enabled)
        {
            // Apply damage to the player's health
            playerHealth.TakeDamage(damageAmount);
        }
        // else
        // {
        //     Debug.LogWarning("PlayerHealth component is null, invalid, or disabled.");
        // }
    }
}
