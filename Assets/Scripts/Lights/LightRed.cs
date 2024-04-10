using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightRed : MonoBehaviour
{
    public LayerMask targetLayer; // Layer to detect
    public float detectionRadius = 5f; // Radius to detect the target layer
    public Color detectionColor = Color.red; // Color to change the light when target is detected

    private Color originalColor;
    private bool targetDetected = false; // Flag to track if target is detected

    private void Start()
    {
        originalColor = Color.white; // Set original color (assuming default color is white)
    }

    private void Update()
    {
        DetectTarget();
    }

    private void DetectTarget()
    {
        // Check for the target layer within the detection radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, targetLayer);
        if(colliders.Length > 0 && !targetDetected)
        {
            targetDetected = true;
        } // Update the flag based on detection result

        if (colliders.Length > 0)
        {
            

                Light2D light2D = GetComponent<Light2D>();
                if (light2D != null)
                {
                    light2D.color = targetDetected ? detectionColor : originalColor;
                }

        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a wire sphere to visualize the detection radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
