using UnityEngine;
using System.Collections.Generic;

public class GravityManager : MonoBehaviour
{
    [Header("Physics Settings")]
    public float gravityConstant = 39.478f; // UA^3 / (year^2 * M☉)
    public float timeStep = 0.01f;
    public float timeScale = 1f;

    [Header("Planets in Simulation")]
    public List<Planet> planets;

    private float ScaledDeltaTime => timeStep * timeScale;

    void Start()
    {
        foreach (var planet in planets)
        {
            planet.Initialize(ScaledDeltaTime);
        }
    }

    void FixedUpdate()
    {
        CalculateAccelerations();
        UpdatePlanetPositions();
    }

    private void CalculateAccelerations()
    {
        foreach (var p in planets)
        {
            Vector3 netForce = Vector3.zero;

            foreach (var other in planets)
            {
                if (ReferenceEquals(p, other)) continue;

                Vector3 offset = other.transform.position - p.transform.position;
                float distanceSqr = offset.sqrMagnitude;

                if (distanceSqr < 1e-6f) continue;

                float forceMagnitude = gravityConstant * p.mass * other.mass / distanceSqr;
                netForce += forceMagnitude * offset.normalized;
            }

            p.acceleration = netForce / p.mass;
        }
    }

    private void UpdatePlanetPositions()
    {
        foreach (var planet in planets)
        {
            planet.VerletUpdate(ScaledDeltaTime);
        }
    }
}