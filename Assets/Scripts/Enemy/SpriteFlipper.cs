using UnityEngine;

public class SpriteFlipper : MonoBehaviour
{
    public Rigidbody2D targetRigidbody; // Reference to the Rigidbody whose velocity will be checked
    public Transform playerTransform; // Reference to the player's transform
    public bool isShooterMode = false; // Flag to indicate if in ShooterMode
    public float rotationSpeed = 500f; // Speed of rotation

    private Quaternion targetRotation;

    private void Update()
    {
        if (isShooterMode)
        {
            // Rotate to face the player
            Vector2 direction = playerTransform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Adjust rotation based on player position relative to the enemy
            if (direction.x > 0)
            {
                // Player is to the right of the enemy
                targetRotation = Quaternion.Euler(0, 180, -angle);
            }
            else
            {
                // Player is to the left of the enemy
                targetRotation = Quaternion.Euler(0, 0, angle + 180);
            }

            // Apply rotation using RotateTowards
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else if (targetRigidbody != null)
        {
            float currentVelocityX = targetRigidbody.velocity.x;

            if (Mathf.Abs(currentVelocityX) > 0.1f)
            {
                // Determine the target rotation based on the velocity direction
                if (currentVelocityX > 0)
                {
                    targetRotation = Quaternion.Euler(0, 180, 0); // Facing right
                }
                else if (currentVelocityX < 0)
                {
                    targetRotation = Quaternion.Euler(0, 0, 0); // Facing left
                }
            }

            // Apply rotation without using RotateTowards
            transform.rotation = targetRotation;
        }
        else
        {
            Debug.LogWarning("Target Rigidbody is not set.");
        }
    }
}
