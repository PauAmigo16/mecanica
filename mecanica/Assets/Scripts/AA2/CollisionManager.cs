using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public static CollisionManager Instance;

    private List<Surface> surfaces = new List<Surface>();
    private List<Obstacle> obstacles = new List<Obstacle>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterSurface(Surface s)
    {
        if (!surfaces.Contains(s)) surfaces.Add(s);
    }

    public void RegisterObstacle(Obstacle o)
    {
        if (!obstacles.Contains(o)) obstacles.Add(o);
    }

    public Surface GetSurfaceUnderBall(Vector3 ballPosition)
    {
        foreach (var surface in surfaces)
        {
            if (surface.IsBallOnSurface(ballPosition))
                return surface;
        }
        return null;
    }
    public List<Surface> GetAllSurfaces()
    {
        return surfaces;
    }

    public List<Obstacle> GetAllObstacles()
    {
        return obstacles;
    }
}