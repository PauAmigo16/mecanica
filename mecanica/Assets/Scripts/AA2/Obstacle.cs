using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Vector3 normal;

    public Vector3 GetNormal()
    {
        return normal;
    }

    private void Start()
    {
        CollisionManager.Instance.RegisterObstacle(this);
        Debug.Log("Obstacle registrat: " + gameObject.name);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + GetNormal());
    }
}