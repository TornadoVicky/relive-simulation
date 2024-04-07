using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Transform patrolPoint1;
    public Transform patrolPoint2;
    public Transform player;
    public float standingDuration = 3f;
    public float patrollingDuration = 5f;
    public float patrolStandDuration = 2f; // New variable for standing at a patrol point
    public float tiltSpeedStanding = 10f;
    public float tiltSpeedPatrolling = 20f;
    public float tiltSpeedRunning = 30f;
    public float patrolSpeed = 3f;
    public float runSpeedMultiplier = 1.5f;

    private Rigidbody2D rb;
    private float tiltAngle;
    private float timeSinceLastChange;
    private float timeSinceLastPatrolChange; // New variable for tracking patrol change
    private float timeSinceLastPatrolStand;  // New variable for tracking time at a patrol point
    private bool isRunning = false;

    private enum EnemyMode
    {
        Standing,
        Patrolling,
        Running
    }

    private EnemyMode mode;
    private Transform currentPatrolPoint;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mode = EnemyMode.Standing;
        currentPatrolPoint = patrolPoint1;
        timeSinceLastChange = Time.time;
        timeSinceLastPatrolChange = Time.time;
        timeSinceLastPatrolStand = Time.time;
    }

    private void Update()
    {
        // Check if it's time to change mode
        if (Time.time - timeSinceLastChange > GetDurationForMode())
        {
            ChangeMode();
        }

        // Update tilt angle based on mode
        UpdateTilt();

        // Move the enemy based on mode
        Move();
    }

    private void UpdateTilt()
    {
        float tiltSpeed = GetTiltSpeedForMode();
        tiltAngle = Mathf.Sin(Time.time * tiltSpeed) * 10f;
        transform.rotation = Quaternion.Euler(0, 0, tiltAngle);
    }

    private void Move()
    {
        float speed = GetSpeedForMode();
        switch (mode)
        {
            case EnemyMode.Standing:
                rb.velocity = Vector2.zero; // Stand still
                break;
            case EnemyMode.Patrolling:
                MoveTowardsPatrolPoint(speed);
                break;
            case EnemyMode.Running:
                MoveAwayFromPlayer(speed);
                break;
        }
    }

    private void MoveTowardsPatrolPoint(float speed)
    {
        // Check if it's time to stand at the patrol point
        if (Time.time - timeSinceLastPatrolChange > patrollingDuration)
        {
            if (Time.time - timeSinceLastPatrolStand > patrolStandDuration)
            {
                SwitchPatrolPoint();
            }
        }

        Vector2 direction = (currentPatrolPoint.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    private void MoveAwayFromPlayer(float speed)
{
    Vector2 toPoint1 = (patrolPoint1.position - transform.position).normalized;
    Vector2 toPoint2 = (patrolPoint2.position - transform.position).normalized;
    Vector2 runDirection = (player.position - transform.position).normalized;

    float angleToPoint1 = Vector2.Angle(runDirection, toPoint1);
    float angleToPoint2 = Vector2.Angle(runDirection, toPoint2);

    Vector2 awayDirection = angleToPoint1 > angleToPoint2 ? toPoint1 : toPoint2;
    rb.velocity = awayDirection * speed * runSpeedMultiplier;
}


    private void SwitchPatrolPoint()
    {
        currentPatrolPoint = currentPatrolPoint == patrolPoint1 ? patrolPoint2 : patrolPoint1;
        timeSinceLastPatrolChange = Time.time;
        timeSinceLastPatrolStand = Time.time;
    }

    public void ChangeToRunningMode()
    {
        mode = EnemyMode.Running;
        timeSinceLastChange = Time.time; // Reset the timer when changing to Running mode
    }

    private void ChangeMode()
    {
        switch (mode)
        {
            case EnemyMode.Standing:
                mode = EnemyMode.Patrolling;
                break;
            case EnemyMode.Patrolling:
                mode = EnemyMode.Standing;
                break;
            case EnemyMode.Running:
                // In the final game, conditions to activate running mode should be implemented
                mode = EnemyMode.Standing;
                break;
        }
        timeSinceLastChange = Time.time;
    }

    private float GetDurationForMode()
    {
        return mode == EnemyMode.Standing ? standingDuration : patrollingDuration;
    }

    private float GetTiltSpeedForMode()
    {
        switch (mode)
        {
            case EnemyMode.Standing:
                return tiltSpeedStanding;
            case EnemyMode.Patrolling:
                return tiltSpeedPatrolling;
            case EnemyMode.Running:
                return tiltSpeedRunning;
            default:
                return tiltSpeedStanding;
        }
    }

    private float GetSpeedForMode()
    {
        return mode == EnemyMode.Patrolling ? patrolSpeed : patrolSpeed * runSpeedMultiplier;
    }
}
