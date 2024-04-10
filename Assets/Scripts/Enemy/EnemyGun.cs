using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    public bool shootModeActive = false;
    public float shootInterval = 1f; // Time interval between shots
    public int damageAmount = 10;
    public ParticleSystem shootEffect; // Reference to the particle effect system
    public Collider2D damageCollider; // Reference to the damage collider
    public AudioSource FlamethrowerAudioSource;

    private float shootTimer;

    private void Update()
    {
        if (shootModeActive)
        {
            // Update the shoot timer
            shootTimer += Time.deltaTime;

            // Check if it's time to shoot based on the shoot interval
            if (shootTimer >= shootInterval)
            {
                //Shoot();
                shootTimer = 0f; // Reset the timer
            }
        }
    }

    public void ActivateShootMode()
    {
        shootModeActive = true;
        shootTimer = 0f; // Start shooting immediately

        // Play the shoot effect if it's not null
        if (shootEffect != null)
        {
            shootEffect.Play();
            FlamethrowerAudioSource.Play();
        }
    }

    public void DeactivateShootMode()
    {
        shootModeActive = false;

        // Stop the shoot effect if it's not null
        if (shootEffect != null)
        {
            shootEffect.Stop();
            FlamethrowerAudioSource.Stop();
        }
    }

    private void Shoot()
    {
        if (damageCollider == null)
        {
            return;
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(damageCollider.transform.position, damageCollider.bounds.extents.magnitude);
        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                PlayerHealth playerHealth = col.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageAmount);
                }
            }
        }
    }
}
