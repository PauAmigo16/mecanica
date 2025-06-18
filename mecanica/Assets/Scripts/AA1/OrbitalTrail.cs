using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class OrbitTrail : MonoBehaviour
{
    [Header("Trail Settings")]
    public int maxPoints = 5000;
    public float minDistance = 0.01f;

    private LineRenderer line;
    private List<Vector3> trailPoints = new List<Vector3>();
    private Vector3 prevPosition;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        Vector3 initial = transform.position;
        trailPoints.Add(initial);
        prevPosition = initial;
    }

    void Update()
    {
        Vector3 currentPos = transform.position;
        if ((currentPos - prevPosition).sqrMagnitude > minDistance * minDistance)
        {
            if (trailPoints.Count >= maxPoints)
                trailPoints.RemoveAt(0);

            trailPoints.Add(currentPos);
            line.positionCount = trailPoints.Count;
            line.SetPositions(trailPoints.ToArray());
            prevPosition = currentPos;
        }
    }

    public void ResetTrail()
    {
        trailPoints.Clear();
        line.positionCount = 0;
    }
}