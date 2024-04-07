using UnityEngine;

public class SpriteFlipper : MonoBehaviour
{
    public Rigidbody2D targetRigidbody; // Reference to the Rigidbody whose velocity will be checked
    public float velocityChangeThreshold = 0.1f; // Minimum velocity change for rotation
    public float rotationLerpSpeed = 5f; // Speed of rotation interpolation

    private Quaternion targetRotation;

    private void Update()
    {
        if (targetRigidbody != null)
        {
            float currentVelocityX = targetRigidbody.velocity.x;

            if (Mathf.Abs(currentVelocityX) > velocityChangeThreshold)
            {
                // Determine the target rotation based on the velocity direction
                if (targetRigidbody.velocity.x > 0)
                {
                    targetRotation = Quaternion.Euler(0, 180, 0); // Facing right
                }
                else if (targetRigidbody.velocity.x < 0)
                {
                    targetRotation = Quaternion.Euler(0, 0, 0); // Facing left
                }

                // Smoothly rotate towards the target rotation using lerp
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationLerpSpeed * Time.deltaTime);
            }
        }
        else
        {
            Debug.LogWarning("Target Rigidbody is not set.");
        }
    }
}
