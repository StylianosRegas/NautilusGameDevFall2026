using Pathfinding;
using UnityEngine;

public class InterceptingMobAI : MonoBehaviour
{
    public Transform target;
    public Transform GFX;

    [Header("Movement")]
    public float maxSpeed = 4f;
    public float steerStrength = 5f;
    public float nextWayPointDistance = 0.5f;

    [Header("Arrival")]
    public float slowDownDistance = 1.5f;
    [Range(0.1f, 1f)]
    public float minSpeedFactor = 0.5f;

    [Header("Interception")]
    public float maxLeadTime = 2f;        // never predicts further ahead than this (seconds)
    public float velocitySmoothing = 10f; // higher = reacts faster to player direction changes
    public float repathInterval = 0.2f;

    Path path;
    int currentWaypoint = 0;
    Seeker seeker;
    Rigidbody2D rb;
    Rigidbody2D targetRb;

    Vector2 targetVelocity;
    Vector2 lastTargetPos;
    Vector2 interceptPoint;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        targetRb = target.GetComponent<Rigidbody2D>();

        lastTargetPos = target.position;
        interceptPoint = target.position;

        InvokeRepeating(nameof(UpdatePath), 0f, repathInterval);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, interceptPoint, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void UpdateTargetVelocity()
    {
        Vector2 measured;

        if (targetRb != null)
        {
            measured = targetRb.linearVelocity;  // use rb.velocity on older Unity versions
        }
        else
        {
            // No rigidbody on the player: estimate velocity from position changes
            measured = ((Vector2)target.position - lastTargetPos) / Time.fixedDeltaTime;
        }

        lastTargetPos = target.position;
        targetVelocity = Vector2.Lerp(targetVelocity, measured, velocitySmoothing * Time.fixedDeltaTime);
    }

    Vector2 ComputeInterceptPoint()
    {
        Vector2 targetPos = target.position;
        Vector2 r = targetPos - rb.position;
        Vector2 v = targetVelocity;
        float s = maxSpeed;

        // Solve |r + v*t| = s*t for the earliest positive t
        float a = Vector2.Dot(v, v) - s * s;
        float b = 2f * Vector2.Dot(r, v);
        float c = Vector2.Dot(r, r);

        float t = -1f;

        if (Mathf.Abs(a) < 0.0001f)
        {
            if (Mathf.Abs(b) > 0.0001f)
                t = -c / b;
        }
        else
        {
            float disc = b * b - 4f * a * c;
            if (disc >= 0f)
            {
                float sqrt = Mathf.Sqrt(disc);
                float t1 = (-b + sqrt) / (2f * a);
                float t2 = (-b - sqrt) / (2f * a);

                if (t1 > 0f && t2 > 0f) t = Mathf.Min(t1, t2);
                else t = Mathf.Max(t1, t2);
            }
        }

        // Can't catch the player (they're faster, or moving away): just lead by travel time
        if (t <= 0f)
            t = r.magnitude / Mathf.Max(s, 0.01f);

        t = Mathf.Min(t, maxLeadTime);

        return targetPos + v * t;
    }

    void FixedUpdate()
    {
        UpdateTargetVelocity();
        interceptPoint = ComputeInterceptPoint();

        if (path == null)
        {
            return;
        }

        float distToIntercept = Vector2.Distance(rb.position, interceptPoint);
        float speedFactor = Mathf.Clamp(distToIntercept / slowDownDistance, minSpeedFactor, 1f);
        Vector2 desiredVelocity;

        if (currentWaypoint < path.vectorPath.Count)
        {
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            desiredVelocity = direction * maxSpeed * speedFactor;

            if (Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]) < nextWayPointDistance)
            {
                currentWaypoint++;
            }
        }
        else
        {
            // End of path: head straight to the intercept point
            Vector2 direction = (interceptPoint - rb.position).normalized;
            desiredVelocity = direction * maxSpeed * speedFactor;
        }

        rb.AddForce((desiredVelocity - rb.linearVelocity) * steerStrength);

        // Face the player
        Vector2 toTarget = (Vector2)target.position - rb.position;
        float angle = Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg;
        GFX.rotation = Quaternion.Euler(0f, 0f, angle);

        if (toTarget.x < 0f)
        {
            GFX.localScale = new Vector3(.1f, -.1f, .1f);
        }
        else
        {
            GFX.localScale = new Vector3(.1f, .1f, .1f);
        }
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interceptPoint, 0.3f);
        Gizmos.DrawLine(transform.position, interceptPoint);
    }
}