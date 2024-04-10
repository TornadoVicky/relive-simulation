using UnityEngine;

public class DoorThrow : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision involves an object with the "Enemy" tag
        if (collision.collider.CompareTag("Enemy"))
        {
            // Check if the collided object has the EnemyDestroyer script
            EnemyDestroyer enemyDestroyer = collision.collider.GetComponent<EnemyDestroyer>();
            if (enemyDestroyer != null)
            {
                // Enable the EnemyDestroyer script
                enemyDestroyer.enabled = true;
            }
        }
    }
}
