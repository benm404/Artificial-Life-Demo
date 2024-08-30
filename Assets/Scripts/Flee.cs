using UnityEngine;

public class Flee : SteeringBehavior
{
    [SerializeField] private Transform enemyTransform;
    [SerializeField] private float fleeRange = 50f;

    private Vector3 force = Vector3.zero;

    public override void OnDrawGizmos()
    {
        if (boid != null && drawGizmos)
        {
            // Draw the sphere indicating the flee range
            Gizmos.color = ColorUtility.TryParseHtmlString("#FA8072", out var color) ? color : Color.red; // Dark Salmon color
            Gizmos.DrawWireSphere(transform.position, fleeRange);

            // Draw the arrow if the force is not zero
            if (force != Vector3.zero)
            {
                Gizmos.color = color;
                Gizmos.DrawLine(transform.position, enemyTransform.position);
            }
        }
    }

    public override Vector3 Calculate()
    {
        if (boid != null && enemyTransform != null)
        {
            Vector3 toEnemy = enemyTransform.position - boid.transform.position;
            float distance = toEnemy.magnitude;

            if (distance < fleeRange)
            {
                toEnemy = toEnemy.normalized;
                Vector3 desired = toEnemy * boid.maxSpeed;
                return boid.velocity - desired;
            }
        }

        return Vector3.zero;
    }

    public override void Awake()
    {
        boid = GetComponentInParent<Boid>();
        // Find enemy transform if set via serialized field
        if (enemyTransform == null)
        {
            // Handle null case if necessary
            Debug.LogWarning("Enemy Transform is not assigned.");
        }
    }
}
