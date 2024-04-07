using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTentacle : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private List<RopeSegment> ropeSegments = new List<RopeSegment>();
    public float ropeSegLen = 0.25f;
    public int segmentLength = 35;
    public float lineWidth = 0.1f;

    public Transform fixedPoint;

    private bool isLatched = false;
    private GameObject latchedObject = null;
    public LayerMask objectLayerMask;

    private Vector2 previousMousePosition;
    private Vector2 cursorVelocity;
    public float ForceScalingFactor = 0.1f;
    public float maxDistance = 5f; // Adjust this value as needed
    public float DetachmentActivationSpeed = 3f;

    // Use this for initialization
    void Start()
    {
        this.lineRenderer = this.GetComponent<LineRenderer>();
        Vector3 ropeStartPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        ropeSegments.Add(new RopeSegment(fixedPoint.position)); // Set the fixed point as the starting position
        for (int i = 1; i < segmentLength; i++)
        {
            ropeSegments.Add(new RopeSegment(ropeSegments[i - 1].posNow - Vector2.up * ropeSegLen)); // Adjust the position of the other segments
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.DrawRope();

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hitCollider = Physics2D.OverlapCircle(mousePosition, 0.1f, objectLayerMask);

            if (hitCollider != null)
            {
                LatchOntoObject(hitCollider.gameObject);
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            DetachLatchedObject();
        }

        if (isLatched)
        {
            Vector2 currentMousePosition = Input.mousePosition;
            cursorVelocity = (currentMousePosition - previousMousePosition) / Time.deltaTime;
            UpdateLatchedObjectPosition();
        }

        previousMousePosition = Input.mousePosition;
    }

    private void FixedUpdate()
    {
        this.Simulate();
    }

    private void UpdateLatchedObjectPosition()
    {
        if (latchedObject != null)
        {
            RopeSegment lastSegment = ropeSegments[ropeSegments.Count - 1];
            latchedObject.transform.position = lastSegment.posNow;
        }
    }

    private void LatchOntoObject(GameObject targetObject)
    {
        if (((1 << targetObject.layer) & objectLayerMask) != 0)
        {
            float distanceToTarget = Vector3.Distance(fixedPoint.position, targetObject.transform.position);

            if (distanceToTarget <= maxDistance)
            {
                isLatched = true;
                latchedObject = targetObject;
                RopeSegment lastSegment = ropeSegments[ropeSegments.Count - 1];
                lastSegment.posNow = targetObject.transform.position;
                ropeSegments[ropeSegments.Count - 1] = lastSegment;
            }
        }
    }



    private void DetachLatchedObject()
{
    if (isLatched && latchedObject != null)
    {
        Rigidbody2D rb2d = latchedObject.GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            // Get the velocity of the last rope segment
            Vector2 lastSegmentVelocity = (ropeSegments[segmentLength - 1].posNow - ropeSegments[segmentLength - 1].posOld) / Time.fixedDeltaTime;

            // Calculate force direction and magnitude based on the last segment's velocity
            Vector2 forceDirection = lastSegmentVelocity.normalized;
            float forceMagnitude = lastSegmentVelocity.magnitude * ForceScalingFactor;

            // Apply the force to the latched object
            rb2d.AddForce(forceDirection * forceMagnitude, ForceMode2D.Impulse);

            EnemyDestroyer enemyDestroyer = latchedObject.GetComponent<EnemyDestroyer>();
            if (enemyDestroyer != null && lastSegmentVelocity.magnitude > DetachmentActivationSpeed)
            {
                enemyDestroyer.enabled = true;
            }
        }

        isLatched = false;
        latchedObject = null;
    }
}


    private void Simulate()
    {
        Vector2 forceGravity = new Vector2(0f, -1.5f);

        for (int i = 1; i < this.segmentLength; i++)
        {
            RopeSegment firstSegment = this.ropeSegments[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity;
            firstSegment.posNow += forceGravity * Time.fixedDeltaTime;
            this.ropeSegments[i] = firstSegment;
        }

        for (int i = 0; i < 50; i++)
        {
            this.ApplyConstraint();
        }
    }

    private void ApplyConstraint()
    {
        // Constrain the rope to fixed point
        RopeSegment firstSegment = ropeSegments[0];
        firstSegment.posNow = fixedPoint.position;
        ropeSegments[0] = firstSegment;

        // Update the position of the last rope segment
        RopeSegment lastSegment = ropeSegments[ropeSegments.Count - 1];
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the distance between the fixed point and the target position
        float distanceToTarget = Vector2.Distance(fixedPoint.position, targetPosition);
        
        // Enforce the maximum distance constraint
        if (distanceToTarget > maxDistance)
        {
            Vector3 direction = (targetPosition - fixedPoint.position).normalized;
            lastSegment.posNow = fixedPoint.position + direction * maxDistance;
        }
        else
        {
            lastSegment.posNow = targetPosition;
        }

        ropeSegments[ropeSegments.Count - 1] = lastSegment;

        // Constraints between rope segments
        for (int i = 0; i < ropeSegments.Count - 1; i++)
        {
            RopeSegment firstSeg = ropeSegments[i];
            RopeSegment secondSeg = ropeSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = Mathf.Abs(dist - ropeSegLen);
            Vector2 changeDir = Vector2.zero;

            if (dist > ropeSegLen)
            {
                changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
            }
            else if (dist < ropeSegLen)
            {
                changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
            }

            Vector2 changeAmount = changeDir * error;
            if (i != 0)
            {
                firstSeg.posNow -= changeAmount * 0.5f;
                ropeSegments[i] = firstSeg;
            }
            secondSeg.posNow += changeAmount * 0.5f;
            ropeSegments[i + 1] = secondSeg;
        }
    }

    private void DrawRope()
    {
        float lineWidth = this.lineWidth;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        Vector3[] ropePositions = new Vector3[this.segmentLength];
        for (int i = 0; i < this.segmentLength; i++)
        {
            ropePositions[i] = this.ropeSegments[i].posNow;
        }

        lineRenderer.positionCount = ropePositions.Length;
        lineRenderer.SetPositions(ropePositions);
    }

    public struct RopeSegment
    {
        public Vector2 posNow;
        public Vector2 posOld;

        public RopeSegment(Vector2 pos)
        {
            this.posNow = pos;
            this.posOld = pos;
        }
    }
}