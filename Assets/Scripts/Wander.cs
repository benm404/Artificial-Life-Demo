using UnityEngine;

public class Wander : SteeringBehavior
{
    [SerializeField] private float distance = 20f;
    [SerializeField] private float radius = 10f;
    [SerializeField] private float jitter = 50f;

    public enum Axis { Horizontal, Vertical }
    [SerializeField] private Axis axis = Axis.Horizontal;

    private Vector3 target;
    private Vector3 worldTarget;
    private Vector3 wanderTarget;
    //private Boid boid;

    private void Start()
    {
        //boid = GetComponentInParent<Boid>();
        wanderTarget = Utils.RandomPointInUnitSphere() * radius;  // Initialize wander target
    }

    public override void OnDrawGizmos()
    {
        if (boid == null) return;
        if (drawGizmos)
        {
            Vector3 center = boid.transform.position + boid.transform.forward * distance;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(center, radius); // Draw the circle representing the wander area
            Gizmos.DrawLine(boid.transform.position, center);  // Line to the center

            Gizmos.DrawLine(center, worldTarget);  // Line to the current world target
        }
    }

    public override Vector3 Calculate()
    {
        float deltaTime = Time.deltaTime;

        // Add jitter to the wander target
        Vector3 displacement = Utils.RandomPointInUnitSphere() * jitter * deltaTime;
        wanderTarget += displacement;

        // Ensure the wander target stays within the circle
        wanderTarget = Vector3.ClampMagnitude(wanderTarget, radius);

        // Convert local wander target to world position
        Vector3 localTarget = boid.transform.forward * distance + wanderTarget;
        worldTarget = boid.transform.position + localTarget;

        // Seek towards the calculated world target
        return boid.Seek(worldTarget);
    }
}