using UnityEngine;

public class Planet : MonoBehaviour
{
    public float mass = 1f;
    public Vector3 initialVelocity;

    [HideInInspector] public Vector3 previousPosition;
    [HideInInspector] public Vector3 acceleration;

    private float defaultMass;
    private Vector3 defaultPosition;
    private Vector3 defaultVelocity;

    public void VerletUpdate(float timeStep)
    {
        Vector3 currentPosition = transform.position;
        Vector3 newPosition = 2 * currentPosition - previousPosition + acceleration * timeStep * timeStep;
        previousPosition = currentPosition;
        transform.position = newPosition;
    }

    public void Initialize(float timeStep)
    {
        SaveDefaults();
        Vector3 currentPosition = transform.position;
        previousPosition = currentPosition - initialVelocity * timeStep;
    }

    public void SaveDefaults()
    {
        defaultMass = mass;
        defaultPosition = transform.position;
        defaultVelocity = initialVelocity;
    }

    public void ResetToDefaults(float timeStep)
    {
        mass = defaultMass;
        transform.position = defaultPosition;
        initialVelocity = defaultVelocity;
        previousPosition = defaultPosition - initialVelocity * timeStep;
        OrbitTrail trail = GetComponent<OrbitTrail>();
        if (trail != null)
        {
            trail.ResetTrail();
        }
    }

    public void ReApplyVelocity (float timeStep)
    {
        Vector3 currentPosition = transform.position;
        previousPosition = currentPosition - initialVelocity * timeStep;
    }
}