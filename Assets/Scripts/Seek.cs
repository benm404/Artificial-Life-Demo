using UnityEngine;

public class Seek : SteeringBehavior
{
    public Transform target; // Reference to the target Transform
    private Vector3 worldTarget;
    [SerializeField] private float seekRange = 50f;

    // Method to draw gizmos in the Scene view
    public override void OnDrawGizmos()
    {
        if (target != null && drawGizmos)
        {
            worldTarget = target.position;
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(target.position, 0.5f); // Visualize the target
            Gizmos.DrawLine(transform.position, worldTarget); // Draw a line from the boid to the target
            Gizmos.DrawWireSphere(transform.position, seekRange); // Visualize seek range
        }
    }

    // Calculate the steering force to seek the target
    public override Vector3 Calculate()
    {
        if (target != null)
        {
            Vector3 toTarget = target.position - boid.transform.position;
            float distance = toTarget.magnitude;

            if (distance < seekRange)
            {
                worldTarget = target.position;
                return boid.Seek(worldTarget);
            }
        }
        return Vector3.zero;
    }
}