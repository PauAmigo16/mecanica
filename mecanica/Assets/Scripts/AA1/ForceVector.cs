using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ForceVector : MonoBehaviour
{
    public Planet planet;
    public float scale = 6500f;

    LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        if (line != null)
            line.positionCount = 2;
    }

    void Update()
    {
        if (planet == null)
        {
            return;
        }

        Vector3 origin = transform.position;
        Vector3 force = planet.acceleration * planet.mass * scale;
        Vector3 target = origin + force;

        line.SetPosition(0, origin);
        line.SetPosition(1, target);
    }
}