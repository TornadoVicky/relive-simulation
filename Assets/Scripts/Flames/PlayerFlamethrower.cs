using UnityEngine;

public class PlayerFlamethrower : MonoBehaviour
{
    public Collider2D triggerCollider; // Reference to the Collider2D
    public ParticleSystem flamethrowerEffect; // Reference to the ParticleSystem
    public LayerMask detectionLayer; // LayerMask to detect
    public AudioSource FlamethrowerAudioSource;

    private void Start()
    {
        // Make sure the ParticleSystem is not playing initially
        if (flamethrowerEffect != null)
        {
            flamethrowerEffect.Stop();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the entering collider is on the specified layer
        if ((detectionLayer & (1 << other.gameObject.layer)) != 0)
        {
            // Start playing the flamethrower effect
            if (flamethrowerEffect != null)
            {
                flamethrowerEffect.Play();
                FlamethrowerAudioSource.Play();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the exiting collider is on the specified layer
        if ((detectionLayer & (1 << other.gameObject.layer)) != 0)
        {
            // Stop playing the flamethrower effect
            if (flamethrowerEffect != null)
            {
                flamethrowerEffect.Stop();
                FlamethrowerAudioSource.Stop();
            }
        }
    }
}
