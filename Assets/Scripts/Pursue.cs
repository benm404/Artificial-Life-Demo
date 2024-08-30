using UnityEngine;

public class Pursue : SteeringBehavior
{
    public Transform enemyBoid;
    private Vector3 projected;

    public override void OnDrawGizmos()
    {
        if (boid != null && drawGizmos && enemyBoid != null)
        {
            Gizmos.color = new Color(0.86f, 0.72f, 0.53f); // Equivalent to Bisque
            Gizmos.DrawLine(boid.transform.position, projected);
        }
    }

    public override Vector3 Calculate()
    {
        if (enemyBoid == null) return Vector3.zero;

        Vector3 toEnemy = enemyBoid.position - boid.transform.position;
        float dist = toEnemy.magnitude;
        float time = dist / boid.maxSpeed;
        projected = enemyBoid.position + (enemyBoid.GetComponent<Boid>().velocity * time);

        return boid.Seek(projected);
    }
}
