using UnityEngine;
using System.Collections.Generic;

public class FlameCollision : MonoBehaviour
{
    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;
    public GameObject collisionEffectPrefab; // Assign the prefab in the Inspector
    public float minDistanceBetweenCollisions = 1.0f; // Minimum distance between collision points
    public float recordedPointDuration = 5.0f; // Duration after which recorded points are removed
    public LayerMask layerToFollow; // Specify the layer to follow

    private List<Vector3> recordedCollisionPoints = new List<Vector3>();
    private List<float> recordedPointTimers = new List<float>();

    private void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void Update()
    {
        UpdateRecordedPointTimers();
    }

    private void UpdateRecordedPointTimers()
    {
        for (int i = recordedPointTimers.Count - 1; i >= 0; i--)
        {
            recordedPointTimers[i] -= Time.deltaTime;
            if (recordedPointTimers[i] <= 0f)
            {
                recordedPointTimers.RemoveAt(i);
                recordedCollisionPoints.RemoveAt(i);
            }
        }
    }

    private void OnParticleCollision(GameObject other)
{
    int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

    int i = 0;
    while (i < numCollisionEvents)
    {
        Vector3 collisionPoint = collisionEvents[i].intersection;
        Vector3 collisionNormal = collisionEvents[i].normal; // Get the collision normal
        if (IsFarEnoughFromRecordedPoints(collisionPoint))
        {
            InstantiateCollisionEffect(collisionPoint, other, collisionNormal); // Pass the collision normal
            recordedCollisionPoints.Add(collisionPoint);
            recordedPointTimers.Add(recordedPointDuration);
        }
        i++;
    }
}


    private bool IsFarEnoughFromRecordedPoints(Vector3 point)
    {
        foreach (Vector3 recordedPoint in recordedCollisionPoints)
        {
            if (Vector3.Distance(point, recordedPoint) < minDistanceBetweenCollisions)
            {
                return false; // Too close to a recorded point
            }
        }
        return true; // Far enough from all recorded points
    }

    private void InstantiateCollisionEffect(Vector3 collisionPoint, GameObject collidedObject, Vector3 collisionNormal)
{
    if (collisionEffectPrefab != null && collidedObject != null &&
        ((1 << collidedObject.layer) & layerToFollow) != 0)
    {
        GameObject effect = Instantiate(collisionEffectPrefab, collisionPoint, Quaternion.identity);

        // Calculate the rotation based on the collision normal
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, collisionNormal);
        effect.transform.rotation = rotation;

        effect.transform.SetParent(collidedObject.transform);
    }
    else
    {
        Debug.LogWarning("Collision effect prefab is not assigned or collided object is null, or layer mismatch.");
    }
}

}
