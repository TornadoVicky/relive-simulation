using UnityEngine;
using System.Collections.Generic;

public class EnemyDestroyer : MonoBehaviour
{
    public GameObject head;
    public GameObject[] eyes;
    public GameObject vision;
    public float movementThreshold = 5f; // Adjust this value as needed

    private Rigidbody2D rbSelf;

    void Start()
    {
        rbSelf = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Check movement threshold of self Rigidbody2D
        if (rbSelf.velocity.magnitude > movementThreshold)
        {
            DestroyAndCleanUp();
        }
    }

    void DestroyAndCleanUp()
    {
        DisableScripts();
        Destroy(vision);
        SetRigidbodySimulation();
        UnparentObjects();
        enabled = false;
    }

    void DisableScripts()
    {
        // Disable scripts on the enemy and head
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            if (script != this) // Exclude this script
                script.enabled = false;
        }
        head.GetComponent<MonoBehaviour>().enabled = false;
    }

    void UnparentObjects()
    {
        // Unparent head and eyes
        head.transform.parent = null;
        foreach (var eye in eyes)
        {
            eye.transform.parent = null;
        }
    }

    void SetRigidbodySimulation()
{
    // Set rigidbody simulation state for referenced GameObjects
    Rigidbody2D rbHead = head.GetComponent<Rigidbody2D>();
    if (rbHead != null)
    {
        rbHead.simulated = true;
    }

    foreach (var eye in eyes)
    {
        Rigidbody2D rbEye = eye.GetComponent<Rigidbody2D>();
        if (rbEye != null)
        {
            rbEye.simulated = true;
        }
    }
}


}
