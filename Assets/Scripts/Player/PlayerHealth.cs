using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;

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
        // Perform actions when the player dies
        Debug.Log("Player has died.");
        // For example, you can deactivate the player gameObject or trigger a game over sequence
        gameObject.SetActive(false);
    }
}
