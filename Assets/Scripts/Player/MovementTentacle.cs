using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MovementTentacle : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private List<RopeSegment> ropeSegments = new List<RopeSegment>();
    public float ropeSegLen = 0.01f;
    private int segmentLength = 35;
    private float lineWidth = 0.2f;

    public Transform fixedPoint;
    private AIDestinationSetter aiDestinationSetter;

    private Vector2 targetInterpolationStartPos;
    private Vector2 targetInterpolationEndPos;
    public float interpolationDuration = 0.5f;
    private float interpolationStartTime;

    public AnimationCurve ropeEndMovementCurve;

    // Use this for initialization
    void Start()
    {
        this.lineRenderer = this.GetComponent<LineRenderer>();
        aiDestinationSetter = GetComponent<AIDestinationSetter>();

        ropeSegments.Add(new RopeSegment(fixedPoint.position));
        for (int i = 1; i < segmentLength; i++)
        {
            ropeSegments.Add(new RopeSegment(ropeSegments[i - 1].posNow - Vector2.up * ropeSegLen));
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.DrawRope();
    }

    private void FixedUpdate()
    {
        this.Simulate();
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
        RopeSegment firstSegment = ropeSegments[0];
        firstSegment.posNow = fixedPoint.position;
        ropeSegments[0] = firstSegment;

        // If the A* pathfinding target position has changed, initiate the interpolation
        Vector2 newTargetPosition = aiDestinationSetter.target.position;
        if (newTargetPosition != targetInterpolationEndPos)
        {
            targetInterpolationStartPos = ropeSegments[ropeSegments.Count - 1].posNow;
            targetInterpolationEndPos = newTargetPosition;
            interpolationStartTime = Time.time;
        }

        float t = Mathf.Clamp01((Time.time - interpolationStartTime) / interpolationDuration);
        Vector2 targetPosition = aiDestinationSetter.target.position;
        Vector2 interpolatedPosition = Vector2.Lerp(targetInterpolationStartPos, targetPosition, ropeEndMovementCurve.Evaluate(t));
        RopeSegment lastSegment = ropeSegments[ropeSegments.Count - 1];
        lastSegment.posNow = interpolatedPosition;
        ropeSegments[ropeSegments.Count - 1] = lastSegment;

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