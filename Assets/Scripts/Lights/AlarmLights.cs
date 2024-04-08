using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AlarmLights : MonoBehaviour
{
    public LayerMask targetLayer; // Layer to detect
    public float detectionRadius = 5f; // Radius to detect the target layer
    public float rotationSpeed = 30f; // Rotation speed on Z axis
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
        RotateObject();
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
            foreach (Transform child in transform)
            {
                Light2D light2D = child.GetComponent<Light2D>();
                if (light2D != null)
                {
                    light2D.color = targetDetected ? detectionColor : originalColor;
                }
            }
        }
    }

    private void RotateObject()
    {
        if (targetDetected)
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a wire sphere to visualize the detection radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
