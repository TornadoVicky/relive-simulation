using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    private bool deathCounterIncreased = false;

    void Start()
    {
        currentHealth = maxHealth; // Set initial health to max health
    }

    // Method to reduce health
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        // Check if health is less than or equal to 0
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method to increase health
    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        // Ensure health does not exceed maxHealth
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }

    void Die()
    {
        // Check if the gameObject's layer is "Player"
        if (gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController playerController = GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.isTentacleMode = false;
                playerController.ToggleAIObjectsAndSelectors(false);
                playerController.enabled = false;
            }

            // Check if the player is within the area of a DeathAreaCounter
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f); // Adjust radius as needed
            foreach (Collider2D col in colliders)
            {
                DeathAreaCounter deathArea = col.GetComponent<DeathAreaCounter>();
                if (deathArea != null && !deathCounterIncreased)
                {
                    deathArea.IncreaseDeathCounter();
                    deathCounterIncreased = true;
                    break; // Exit the loop after increasing the death counter
                }
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
