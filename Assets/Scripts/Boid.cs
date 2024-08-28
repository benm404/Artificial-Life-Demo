/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    float mass = 1;
    Vector3 force = Vector3.zero;
    Vector3 acceleration = Vector3.zero;
    Vector3 velocity = Vector3.zero;
    float speed;
    float max_speed = 10.0f;

    Behaviour[] behaviours = { };

    float max_force = 10.0f;
    float banking = 0.1f;
    float damping = 0.1f;

    bool count_neighbours = false;
    GameObject[] neighbours;
    int neighbour_count = 0;

    Vector3 new_force = Vector3.zero;
    bool should_calculate = false;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Boid"))
        {
            neighbour_count++;
            neighbours[neighbour_count-1] = other.transform.GetComponentInParent<Transform>().GetComponentInParent<GameObject>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Boid"))
        {
            neighbour_count--;
            neighbours[neighbour_count] = null;
        }
    }

    Vector3 seek_force(Vector3 target)
    {
        Vector3 toTarget = target - transform.position;
        toTarget = toTarget.normalized;
        Vector3 desired = toTarget * max_speed;
        return desired - velocity;
    }

    Vector3 arrive_force(Vector3 target, float slowingDistance) {

        Vector3 toTarget = target - transform.position;

        float dist = toTarget.magnitude;


        if(dist < 2) {
            return Vector3.zero;
        }


        float ramped = (dist / slowingDistance) * max_speed;
        float limit_length = Mathf.Min(max_speed, ramped);

        Vector3 desired = (toTarget * limit_length) / dist;

        return desired - velocity;
            }

    void set_enabled_all(bool enabled)
    {
        for(int i = 0; i < behaviours.Length; i++)
        {
            behaviours[i].enabled = enabled;
        }
    }
}
*/
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    [SerializeField] private float mass = 1f;
    [SerializeField] private Vector3 force = Vector3.zero;
    [SerializeField] private Vector3 acceleration = Vector3.zero;
    [SerializeField] private Vector3 velocity = Vector3.zero;
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed = 10f;

    private List<SteeringBehavior> behaviors = new List<SteeringBehavior>();
    [SerializeField] private float maxForce = 10f;
    [SerializeField] private float banking = 0.1f;
    [SerializeField] private float damping = 0.1f;

    [SerializeField] private bool drawGizmos = true;
    [SerializeField] private bool pause = false;

    private bool countNeighbors = false;
    private List<Boid> neighbors = new List<Boid>();

    private School school;
    private Vector3 newForce = Vector3.zero;
    private bool shouldCalculate = false;

    // Gizmos for debug drawing
    private void OnDrawGizmos()
    {
        if (!drawGizmos)
            return;
        
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 10.0f);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.right * 10.0f);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.up * 10.0f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + force);

            if (school != null && countNeighbors)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(transform.position, school.neighborDistance);
                foreach (var neighbor in neighbors)
                {
                    Gizmos.DrawWireSphere(neighbor.transform.position, 3);
                }
            }
        
    }

    private void Start()
    {
        // Initialize behaviors and school reference
        school = GetComponentInParent<School>();

        foreach (Transform child in transform)
        {
            var behavior = child.GetComponent<SteeringBehavior>();
            if (behavior != null)
            {
                behaviors.Add(behavior);
                behavior.enabled = behavior.IsEnabled; // Assuming IsEnabled is a bool in SteeringBehavior
            }
        }
    }

    private void Update()
    {
        if (pause) return;

        shouldCalculate = true;

        if (school != null && countNeighbors)
        {
            CountNeighbors();
        }
    }

    private void FixedUpdate()
    {
        if (shouldCalculate)
        {
            newForce = CalculateForces();
            shouldCalculate = false;
        }

        force = Vector3.Lerp(force, newForce, Time.fixedDeltaTime);

        if (!pause)
        {
            acceleration = force / mass;
            velocity += acceleration * Time.fixedDeltaTime;
            speed = velocity.magnitude;

            if (speed > 0)
            {
                velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

                // Apply damping
                velocity -= velocity * Time.fixedDeltaTime * damping;

                // Apply movement
                transform.position += velocity * Time.fixedDeltaTime;

                // Banking effect
                Vector3 tempUp = Vector3.Lerp(transform.up, Vector3.up + acceleration * banking, Time.fixedDeltaTime * 5.0f);
                transform.rotation = Quaternion.LookRotation(velocity.normalized, tempUp);
            }
        }
    }

    private Vector3 CalculateForces()
    {
        Vector3 forceAccumulator = Vector3.zero;
        string behaviorsActive = "";

        foreach (var behavior in behaviors)
        {
            if (behavior.IsEnabled)
            {
                Vector3 behaviorForce = behavior.Calculate() * behavior.Weight;
                if (float.IsNaN(behaviorForce.x) || float.IsNaN(behaviorForce.y) || float.IsNaN(behaviorForce.z))
                {
                    Debug.Log($"{behavior.name} produced NaN force");
                    behaviorForce = Vector3.zero;
                }
                behaviorsActive += $"{behavior.name}: {Mathf.Round(behaviorForce.magnitude)} ";
                forceAccumulator += behaviorForce;
                if (forceAccumulator.magnitude > maxForce)
                {
                    forceAccumulator = Vector3.ClampMagnitude(forceAccumulator, maxForce);
                    behaviorsActive += " Limiting force";
                    break;
                }
            }
        }

        if (drawGizmos)
        {
            // Implement Debug Drawing of behavior info (optional)
        }

        return forceAccumulator;
    }

    private void CountNeighbors()
    {
        neighbors.Clear();

        Collider[] hits = Physics.OverlapSphere(transform.position, school.neighborDistance);
        foreach (var hit in hits)
        {
            Boid boid = hit.GetComponent<Boid>();
            if (boid != null && boid != this)
            {
                neighbors.Add(boid);
                if (neighbors.Count >= school.maxNeighbors)
                    break;
            }
        }
    }

    public Vector3 Seek(Vector3 target)
    {
        Vector3 desired = (target - transform.position).normalized * maxSpeed;
        return desired - velocity;
    }

    public Vector3 Arrive(Vector3 target, float slowingDistance)
    {
        Vector3 toTarget = target - transform.position;
        float distance = toTarget.magnitude;

        if (distance < 2)
            return Vector3.zero;

        float rampedSpeed = Mathf.Min(maxSpeed, (distance / slowingDistance) * maxSpeed);
        Vector3 desired = (toTarget * rampedSpeed) / distance;
        return desired - velocity;
    }
}