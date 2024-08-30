using UnityEngine;

public class Pursue : SteeringBehavior
{
    public Transform[] enemyBoids;
    private Vector3 projected;

    [SerializeField] private float pursueRange = 50f;

    public override void OnDrawGizmos()
    {
        if (boid != null && drawGizmos && enemyBoids != null)
        {
            Gizmos.color = new Color(0.86f, 0.72f, 0.53f); // Equivalent to Bisque
            Gizmos.DrawLine(boid.transform.position, projected);
        }
    }

    public override Vector3 Calculate()
    {
        if (enemyBoids == null) return Vector3.zero;

        Vector3 toEnemy = ClosestEnemy().transform.position - boid.transform.position;
        float dist = toEnemy.magnitude;
        float time = dist / boid.maxSpeed;
        projected = ClosestEnemy().position + (ClosestEnemy().GetComponent<Boid>().velocity * time);

        if (dist < pursueRange)
        {
            return boid.Seek(projected);
        }
        return Vector3.zero;
    }

    private Transform ClosestEnemy()
    {
        Transform closeEnemy = null;
        Vector3 value = Vector3.positiveInfinity;
        int index = -1;
        for (int i = 0; i < prey.Length; i++)
        {
            if ((prey[i].transform.position - boid.transform.position).magnitude < value.magnitude)
            {
                index = i;
                value = prey[i].transform.position;
                closeEnemy = prey[i].transform;
            }
        }
        return closeEnemy;
    }

    private void FixedUpdate()
    {
        for(int i = 0; i < prey.Length; i++)
        {
            enemyBoids = new Transform[prey.Length];
            enemyBoids[i] = prey[i].transform;
        }
    }
}