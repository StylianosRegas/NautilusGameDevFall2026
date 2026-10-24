using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform target;
    public float speed = 50f;
    public float nextWayPointDistance = 3f;
    public Transform InterceptingFishGFX;
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    Seeker seeker;
    Rigidbody2D rb;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 1f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

        Vector2 force = direction * speed * Time.fixedDeltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWayPointDistance)
        {
            currentWaypoint++;
        }

        // Point the head at the player
        Vector2 toTarget = (Vector2)target.position - rb.position;
        float angle = Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg;

        // Assumes the sprite's head faces RIGHT by default (see notes below)
        InterceptingFishGFX.rotation = Quaternion.Euler(0f, 0f, angle);

        // Flip vertically when facing left so the fish isn't upside down
        if (toTarget.x < 0f)
        {
            InterceptingFishGFX.localScale = new Vector3(.1f, -.1f, .1f);
        }
        else
        {
            InterceptingFishGFX.localScale = new Vector3(.1f, .1f, .1f);
        }
    }
}
