using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public Collider2D detectionCollider;
    public EnemyScript enemy;
    public GameObject eye1; // Reference to eye1 GameObject
    public GameObject eye2; // Reference to eye2 GameObject

    private bool playerDetected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to a GameObject with the "Player" layer
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !playerDetected)
        {
            enemy.ChangeToRunningMode();
            playerDetected = true;

            // Disable eye1 and enable eye2
            if (eye1 != null)
            {
                eye1.SetActive(false);
            }
            if (eye2 != null)
            {
                eye2.SetActive(true);
            }
        }
    }
}
