using UnityEngine;

public class DoorBreaker : MonoBehaviour
{
    private new HingeJoint2D hingeJoint;
    private Vector2 lastConnectedAnchor;

    void Start()
    {
        // Get the HingeJoint2D component attached to this object
        hingeJoint = GetComponent<HingeJoint2D>();
        if (hingeJoint == null)
        {
            enabled = false;
            return;
        }

        // Record the initial connected anchor position
        lastConnectedAnchor = hingeJoint.connectedAnchor;
    }

    void Update()
    {
        // Check if the connected anchor's X or Y values have changed
        if (hingeJoint != null && (lastConnectedAnchor != hingeJoint.connectedAnchor))
        {
            // Remove the HingeJoint2D component
            Destroy(hingeJoint);

            // Disable this script
            enabled = false;
        }

        // Update the last connected anchor position
        lastConnectedAnchor = hingeJoint.connectedAnchor;
    }
}
