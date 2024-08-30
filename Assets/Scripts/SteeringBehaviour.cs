using UnityEngine;

public class SteeringBehavior : MonoBehaviour
{
    [SerializeField] public float weight = 1.0f;  // Weight of the behavior
    [SerializeField] public bool drawGizmos = true;  // Whether to draw gizmos for debugging
    [SerializeField] private bool enabledBehavior = true;  // Whether the behavior is enabled

    [HideInInspector] public GameObject[] prey;
    [HideInInspector] public GameObject[] pred;
    [HideInInspector] public Boid boid;

    public virtual void Awake()
    {
        boid = GetComponentInParent<Boid>();
        prey = GameObject.FindGameObjectsWithTag("Prey");
        pred = GameObject.FindGameObjectsWithTag("Pred");
    }

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

    public virtual void OnDrawGizmos()
    {
        // Override this in derived classes to provide custom gizmo drawing
    }

    protected virtual void Update()
    {
            foreach (var behavior in boid.behaviors)
            {
                if (behavior != null && weight >= behavior.weight)
                {
                    drawGizmos = true;
                }
                else drawGizmos = false;
            }
    }

    // Example of a calculate method to be overridden in derived classes
    public virtual Vector3 Calculate()
    {
        return Vector3.zero;
    }
}