using UnityEngine;
using System.Collections.Generic;

public class Avoidance : SteeringBehavior
{
    public enum ForceDirection { Normal, Incident, Up, Braking }
    public ForceDirection direction = ForceDirection.Normal;

    public float feelerAngle = 45f;
    public float feelerLength = 10f;
    public float updatesPerSecond = 5f;

    private Vector3 force = Vector3.zero;
    private List<Feeler> feelers = new List<Feeler>();
    private bool needsUpdating = true;
    private float timer = 0f;

    private void Start()
    {
        timer = Random.Range(0f, 1f);
    }

    private void FixedUpdate()
    {
        timer -= Time.fixedDeltaTime;
        if (timer <= 0f)
        {
            needsUpdating = true;
            timer = 1f / updatesPerSecond;
        }

        if (needsUpdating)
        {
            UpdateFeelers();
            needsUpdating = false;
        }
    }

    private Feeler Feel(Vector3 localRay)
    {
        Feeler feeler = new Feeler();
        Vector3 rayEnd = boid.transform.position + localRay;
        Ray ray = new Ray(boid.transform.position, localRay);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, feelerLength))
        {
            feeler.hit = true;
            feeler.hitTarget = hit.point;
            feeler.normal = hit.normal;
            Vector3 toBoid = boid.transform.position - hit.point;
            float forceMagnitude = (feelerLength - toBoid.magnitude) / feelerLength;

            switch (direction)
            {
                case ForceDirection.Normal:
                    feeler.force = hit.normal * forceMagnitude;
                    break;
                case ForceDirection.Incident:
                    feeler.force = Vector3.Reflect(toBoid.normalized, hit.normal) * forceMagnitude;
                    break;
                case ForceDirection.Up:
                    feeler.force = Vector3.up * forceMagnitude;
                    break;
                case ForceDirection.Braking:
                    feeler.force = toBoid.normalized * forceMagnitude;
                    break;
            }

            force += feeler.force;
        }

        feeler.end = rayEnd;
        return feeler;
    }

    private void UpdateFeelers()
    {
        force = Vector3.zero;
        feelers.Clear();
        Vector3 forwards = boid.transform.forward * feelerLength;
        feelers.Add(Feel(forwards));
        feelers.Add(Feel(Quaternion.Euler(0, feelerAngle, 0) * forwards));
        feelers.Add(Feel(Quaternion.Euler(0, -feelerAngle, 0) * forwards));
        feelers.Add(Feel(Quaternion.Euler(feelerAngle, 0, 0) * forwards));
        feelers.Add(Feel(Quaternion.Euler(-feelerAngle, 0, 0) * forwards));
    }

    public override Vector3 Calculate()
    {
        return force;
    }

    public override void OnDrawGizmos()
    {
        if (boid == null) return;
        if(drawGizmos)
        foreach (Feeler feeler in feelers)
        {
            if (feeler.hit)
            {
                Gizmos.color = Color.green; // Chartreuse equivalent
                Gizmos.DrawLine(boid.transform.position, feeler.hitTarget);
                Gizmos.color = Color.blue;
                Gizmos.DrawRay(feeler.hitTarget, feeler.normal);
                Gizmos.color = Color.red;
                Gizmos.DrawRay(feeler.hitTarget, feeler.force);
            }
            else
            {
                Gizmos.color = Color.green; // Chartreuse equivalent
                Gizmos.DrawLine(boid.transform.position, feeler.end);
            }
        }
    }

    private class Feeler
    {
        public bool hit;
        public Vector3 hitTarget;
        public Vector3 normal;
        public Vector3 force;
        public Vector3 end;
    }
}