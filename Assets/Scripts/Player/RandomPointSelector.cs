using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class RandomPointSelector : MonoBehaviour
{
    public Transform squareBody;
    public Transform idealPosition;
    public LayerMask tileLayer;
    public float detectionRadius = 1.0f;
    private Transform targetTransform;

    private Vector3 currentTarget;
    private float destinationScore = 0.0f;
    private Vector3 selectedPointForComparison;

    public GameObject aiGameObject;
    private AIDestinationSetter aiDestinationSetter;

    public int updateFrequency = 3;
    private int frameCounter = 0;

    private void Start()
    {
        aiDestinationSetter = aiGameObject.GetComponent<AIDestinationSetter>();

        targetTransform = new GameObject().transform;
    }

    private void Update()
    {

        frameCounter++;

        if (frameCounter >= updateFrequency)
        {
            frameCounter = 0;
            List<Vector3> grabbablePoints = CalculateGrabbablePoints();

            if (grabbablePoints.Count > 0)
            {
                Vector3 randomPoint = grabbablePoints[Random.Range(0, grabbablePoints.Count)];

                destinationScore = CalculateScore(currentTarget);
                float score = CalculateScore(randomPoint);

                selectedPointForComparison = randomPoint;

                if (score > destinationScore)
                {
                    currentTarget = randomPoint;
                    destinationScore = score;
                }
            
                targetTransform.position = currentTarget;

                aiDestinationSetter.target = targetTransform;
            }
        }
    }

    List<Vector3> CalculateGrabbablePoints()
    {
        List<Vector3> grabbablePoints = new List<Vector3>();

        int numRays = 12;   //Adjust for accuracy
        for (int i = 0; i < numRays; i++)
        {
            float angle = 360f * (i / (float)numRays);
            Vector3 rayDirection = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0f);
            RaycastHit2D hit = Physics2D.Raycast(squareBody.position, rayDirection, detectionRadius, tileLayer);

            if (hit.collider != null)
            {
                grabbablePoints.Add(hit.point);
            }
        }

        return grabbablePoints;
    }

    float CalculateScore(Vector3 point)
    {
        Vector2 point2D = new Vector2(point.x, point.y);
        Vector2 idealPosition2D = new Vector2(idealPosition.position.x, idealPosition.position.y);
        float squaredDistance = Vector2.SqrMagnitude(point2D - idealPosition2D);
        return -squaredDistance;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(currentTarget, 0.5f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(selectedPointForComparison, 0.5f);
    }
}