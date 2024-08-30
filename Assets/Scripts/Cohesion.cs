using UnityEngine;
using System.Collections.Generic;

public class Cohesion : SteeringBehavior
{
    private Vector3 force = Vector3.zero;
    private Vector3 centerOfMass = Vector3.zero;

    public override void Awake()
    {
        boid = GetComponentInParent<Boid>();
        // Ensure count_neighbors is true
        if (boid != null)
        {
            boid.countNeighbors = true;
        }
    }

    // Called when the scene view is updated
    public override void OnDrawGizmos()
    {
        if (boid != null && drawGizmos)
        {
            Gizmos.color = Color.green; // Using Unity's Color structure
            Gizmos.DrawLine(boid.transform.position, centerOfMass);
        }
    }

    // Calculate the cohesion force
    public override Vector3 Calculate()
    {
        force = Vector3.zero;
        centerOfMass = Vector3.zero;

        if (boid != null)
        {
            foreach (var other in boid.neighbors)
            {
                centerOfMass += other.transform.position;

            }

            if (boid.neighbors.Count > 0)
            {
                centerOfMass /= boid.neighbors.Count;
                force = boid.Seek(centerOfMass).normalized;
            }
        }

        return force;
    }
}
