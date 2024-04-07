using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public Collider2D detectionCollider;
    public EnemyScript enemy;
    public GameObject NormalEye1; // Reference to eye1 GameObject
    public GameObject NormalEye2; // Reference to eye2 GameObject
    public GameObject ScaredEye1; // Reference to eye1 GameObject
    public GameObject ScaredEye2; // Reference to eye2 GameObject

    private bool playerDetected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to a GameObject with the "Player" layer
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !playerDetected)
        {
            enemy.ChangeToRunningMode();
            playerDetected = true;

            // Disable eye1 and enable eye2
            if (NormalEye1 != null && NormalEye2 != null)
            {
                NormalEye1.SetActive(false);
                NormalEye2.SetActive(false);
            }
            if (ScaredEye1 != null && ScaredEye2 != null)
            {
                ScaredEye1.SetActive(true);
                ScaredEye2.SetActive(true);
            }
        }
    }
}
