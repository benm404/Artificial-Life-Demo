using System.Collections.Generic;
using UnityEngine;

public class Separation : SteeringBehavior
{
    private Vector3 force = Vector3.zero;

    private void Start()
    {
        boid.countNeighbors = true;
    }

    public override void OnDrawGizmos()
    {
        if (boid == null) return;

        Gizmos.color = Color.green; // Using Dark Sea Green equivalent in Unity

        foreach (Boid neighbor in boid.neighbors)
        {
            Vector3 toOther = neighbor.transform.position - boid.transform.position;
            toOther.Normalize();
            Gizmos.DrawLine(boid.transform.position, boid.transform.position + toOther * force.magnitude * weight * 5);
        }
    }

    public override Vector3 Calculate()
    {
        force = Vector3.zero;

        foreach (Boid neighbor in boid.neighbors)
        {
            Vector3 away = boid.transform.position - neighbor.transform.position;
            force += away.normalized / away.magnitude;
        }

        return force;
    }
}
