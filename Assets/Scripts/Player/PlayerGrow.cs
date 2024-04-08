using UnityEngine;

public class PlayerGrow : MonoBehaviour
{
    public float attractRadius = 5f; // Radius to attract enemies
    public float innerRadius = 2f; // Radius for inner circle
    public LayerMask enemyLayer; // Layer mask for enemies
    public float attractionSpeedMultiplier = 10f;
    public float moveSpeed = 3f; // Speed at which enemies move towards their target

    private void Update()
    {
        Collider2D[] attractColliders = Physics2D.OverlapCircleAll(transform.position, attractRadius, enemyLayer);

        foreach (Collider2D col in attractColliders)
        {
            Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
            if (rb != null && !rb.isKinematic)
            {
                // Check if the enemy is within the inner radius
                Vector2 directionToPlayer = (Vector2)transform.position - rb.position;
                if (directionToPlayer.sqrMagnitude <= innerRadius * innerRadius)
                {
                    // Set a random target within the inner circle
                    Vector2 randomTarget = (Vector2)transform.position + Random.insideUnitCircle.normalized * innerRadius;

                    // Move towards the random target
                    Vector2 moveDirection = (randomTarget - rb.position).normalized;
                    rb.velocity = moveDirection * moveSpeed;
                }
                else
                {
                    // Attract towards the player
                    Vector2 targetVelocity = directionToPlayer.normalized * attractionSpeedMultiplier;
                    rb.velocity = Vector2.MoveTowards(rb.velocity, targetVelocity, Time.deltaTime * attractionSpeedMultiplier);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw wire spheres to visualize the attract and inner radius
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attractRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, innerRadius);
    }
}
