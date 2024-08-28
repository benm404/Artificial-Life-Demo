using UnityEngine;

public class SteeringBehavior : MonoBehaviour
{
    [SerializeField] private float weight = 1.0f;  // Weight of the behavior
    [SerializeField] private bool drawGizmos = true;  // Whether to draw gizmos for debugging
    [SerializeField] private bool enabledBehavior = true;  // Whether the behavior is enabled

    private Boid boid;

    public float Weight
    {
        get { return weight; }
        set { weight = value; }
    }

    public bool IsEnabled
    {
        get { return enabledBehavior; }
        set
        {
            enabledBehavior = value;
            enabled = enabledBehavior;  // Enable/disable the MonoBehaviour itself
        }
    }

    protected virtual void OnDrawGizmos()
    {
        // Override this in derived classes to provide custom gizmo drawing
    }

    protected virtual void Update()
    {
        if (drawGizmos && enabledBehavior)
        {
            OnDrawGizmos();
        }
    }

    // Example of a calculate method to be overridden in derived classes
    public virtual Vector3 Calculate()
    {
        return Vector3.zero;
    }
}