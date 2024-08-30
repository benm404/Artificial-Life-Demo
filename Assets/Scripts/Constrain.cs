using UnityEngine;

public class Constrain : SteeringBehavior
{
    public float radius = 100f;
    public Transform center;

    private void Start()
    {
        boid = GetComponent<Boid>();

        // Assuming center is assigned through the Inspector or via another script
        // If using a NodePath system like Godot, handle it differently
    }

    public override void OnDrawGizmos()
    {
        if (boid == null) return;
        if (drawGizmos)
        {
            Vector3 centerPos = center != null ? center.position : Vector3.zero;
            Gizmos.color = new Color(0.96f, 0.96f, 0.86f); // BEIGE equivalent
            Gizmos.DrawWireSphere(centerPos, radius);
        }
    }

    public override Vector3 Calculate()
    {
        Vector3 toCenter = center != null ? center.position - boid.transform.position : -boid.transform.position;
        float distance = toCenter.magnitude;
        float power = Mathf.Max(distance - radius, 0);

        return toCenter.normalized * power;
    }
}
