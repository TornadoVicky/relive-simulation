using UnityEngine;
using System.Collections;

public class EnemyVision : MonoBehaviour
{
    public Collider2D detectionCollider;
    public EnemyScript enemy;
    public EnemyGun enemyGun;
    public SpriteFlipper spriteFlipper; // Reference to the SpriteFlipper component
    public GameObject NormalEye1; // Reference to eye1 GameObject
    public GameObject NormalEye2; // Reference to eye2 GameObject
    public GameObject ScaredEye1; // Reference to eye1 GameObject
    public GameObject ScaredEye2; // Reference to eye2 GameObject

    public bool hasGun = true; // Indicates if the enemy has a gun

    private bool playerDetected = false;
    private bool scared = false;

    private Coroutine shootingDelayCoroutine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to a GameObject with the "Player" tag
        if (other.CompareTag("Player") && !playerDetected)
        {
            playerDetected = true;
            enemy.ChangeToRunningMode();

            // Activate shooting mode if the enemy has a gun
            if (hasGun)
            {
                enemyGun.ActivateShootMode();

                // Activate Shooter Mode in SpriteFlipper
                if (spriteFlipper != null)
                {
                    spriteFlipper.isShooterMode = true;
                }
            }

            // Disable eye1 and enable eye2
            if (NormalEye1 != null && NormalEye2 != null && !scared)
            {
                NormalEye1.SetActive(false);
                NormalEye2.SetActive(false);
            }
            if (ScaredEye1 != null && ScaredEye2 != null && !scared)
            {
                ScaredEye1.SetActive(true);
                ScaredEye2.SetActive(true);

                scared = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the collider belongs to a GameObject with the "Player" tag
        if (other.CompareTag("Player") && playerDetected)
        {
            playerDetected = false;

            // Deactivate shooting mode after a delay if the enemy has a gun
            if (hasGun)
            {
                if (shootingDelayCoroutine != null)
                {
                    StopCoroutine(shootingDelayCoroutine);
                }
                shootingDelayCoroutine = StartCoroutine(DeactivateShootingDelayed());
            }
        }
    }

    private IEnumerator DeactivateShootingDelayed()
    {
        yield return new WaitForSeconds(1f); // Change the delay time as needed
        enemyGun.DeactivateShootMode();

        // Deactivate Shooter Mode in SpriteFlipper
        if (spriteFlipper != null)
        {
            spriteFlipper.isShooterMode = false;
        }
    }
}
