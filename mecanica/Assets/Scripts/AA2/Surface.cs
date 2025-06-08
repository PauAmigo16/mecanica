using UnityEngine;

public class Surface : MonoBehaviour
{
    public float friction = 0.3f;
    public Vector2 size = new Vector2(10f, 10f); // Dimensions X-Z de la superfície (rectangle)

    private void Start()
    {
        CollisionManager.Instance.RegisterSurface(this);
    }

    public Vector3 GetNormal()
    {
        return transform.up.normalized;
    }

    public bool IsBallOnSurface(Vector3 ballPosition)
    {
        // Transformem la posició de la bola al sistema local de la superfície
        Vector3 localPos = transform.InverseTransformPoint(ballPosition);

        float halfWidth = size.x / 2f;
        float halfLength = size.y / 2f;

        // Comprovar si la bola està dins dels límits XY locals (X i Z del pla)
        bool insideXZ = Mathf.Abs(localPos.x) <= halfWidth &&
                        Mathf.Abs(localPos.z) <= halfLength;

        // Comprovar si està a prop del pla en Y local (que és la direcció normal)
        bool nearSurface = Mathf.Abs(localPos.y) <= 0.6f; // 0.5f radi + marge

        return insideXZ && nearSurface;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + GetNormal());
    }
}