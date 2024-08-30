using UnityEngine;

public class OffsetPursue : SteeringBehavior
{
    [SerializeField] private Boid leaderBoid;
    private Vector3 leaderOffset;
    private Vector3 worldTarget;
    private Vector3 projected;

    public override void OnDrawGizmos()
    {
        if (leaderBoid != null && boid != null && drawGizmos)
        {
            Gizmos.color = Color.blue; // You might need to set a custom color
            Gizmos.DrawLine(worldTarget, projected);
            Gizmos.color = Color.green; // This is for debugging purposes, if needed
            Gizmos.DrawWireSphere(worldTarget, 1f);
            Gizmos.DrawWireSphere(projected, 1f);
        }
    }

    private void CalculateOffset()
    {
        if (boid != null && leaderBoid != null)
        {
            leaderOffset = boid.transform.position - leaderBoid.transform.position;
            leaderOffset = leaderBoid.transform.TransformVector(leaderOffset);
        }
    }

    private void Start()
    {
        CalculateOffset();
    }

    public override Vector3 Calculate()
    {
        if (boid != null && leaderBoid != null)
        {
            worldTarget = leaderBoid.transform.TransformPoint(leaderOffset);
            float distance = Vector3.Distance(boid.transform.position, worldTarget);
            float time = distance / boid.maxSpeed;
            projected = worldTarget + leaderBoid.velocity * time;

            return boid.Arrive(projected, 30f);
        }

        return Vector3.zero;
    }
}
