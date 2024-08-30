using UnityEngine;

public class Flee : SteeringBehavior
{
    [SerializeField] private Transform[] enemyTransforms;
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
                Gizmos.DrawLine(transform.position, ClosestEnemy(enemyTransforms));
            }
        }
    }

    public override Vector3 Calculate()
    {
        

        if (boid != null && enemyTransforms != null)
        {
            Vector3 toEnemy = ClosestEnemy(enemyTransforms) - boid.transform.position;
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
        if (enemyTransforms == null)
        {
            // Handle null case if necessary
            Debug.LogWarning("Enemy Transform is not assigned.");
        }
    }

    private Vector3 ClosestEnemy(Transform[] enemyTransforms)
    {
        Vector3 value = Vector3.positiveInfinity;
        int index = -1;
        for(int i = 0; i < enemyTransforms.Length; i++)
        {
            if((enemyTransforms[i].transform.position - boid.transform.position).magnitude < value.magnitude)
            {
                index = i;
                value = enemyTransforms[i].transform.position;
            }
        }
        return value;
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < prey.Length; i++)
        {
            enemyTransforms = new Transform[pred.Length];
            enemyTransforms[i] = pred[i].transform;
        }
    }
}
