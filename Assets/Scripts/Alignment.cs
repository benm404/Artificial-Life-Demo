using UnityEngine;

public class Alignment : SteeringBehavior
{
    private Vector3 force = Vector3.zero;
    private Vector3 desired = Vector3.zero;

    // Called when the script instance is being loaded
    public override void Awake()
    {
        boid = transform.parent.GetComponent<Boid>();
        if (boid != null)
        {
            boid.countNeighbors = true;
        }
    }

    public override void OnDrawGizmos()
    {
        if (boid != null && drawGizmos)
        {
            Gizmos.color = Color.grey;
            Gizmos.DrawLine(boid.transform.position, boid.transform.position + desired * weight);
        }
    }

    public override Vector3 Calculate()
    {
        desired = Vector3.zero;
        if (boid.neighbors != null)
        {
            foreach (var neighbor in boid.neighbors)
            {
                desired += neighbor.transform.forward;
            }
            if (boid.neighbors.Count > 0)
            {
                desired /= boid.neighbors.Count;
                force = desired - boid.transform.forward;
            }
        }
        return force;
    }
}
