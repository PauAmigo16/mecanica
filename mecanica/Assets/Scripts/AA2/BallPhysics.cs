using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    public Vector3 velocity;
    public float mass = 1f;
    public Vector3 appliedForce;
    public bool isGrounded = false;

    private Surface currentSurface;
    private float radius = 0.5f;

    void FixedUpdate()
    {
        Surface surface = CollisionManager.Instance.GetSurfaceUnderBall(transform.position);

        if (surface != null)
        {
            SetSurface(surface);
        }
        else
        {
            isGrounded = false;
            currentSurface = null;
        }

        ApplyForces();
        HandleMovement();
        CheckCollision();
    }

    void ApplyForces()
    {
        if (currentSurface == null)
        {
            // En l'aire, només gravetat vertical
            Vector3 Gravity = Vector3.down * 9.81f;
            velocity += Gravity * Time.fixedDeltaTime;
            return;
        }

        Vector3 normal = currentSurface.GetNormal().normalized;
        Vector3 gravity = Vector3.down * 9.81f;

        // Descomposar la gravetat en components normal i paral·lela al pla
        Vector3 gravityNormal = Vector3.Dot(gravity, normal) * normal;
        Vector3 gravityParallel = gravity - gravityNormal;

        // Aplica acceleració només en la direcció paral·lela al pla
        velocity += gravityParallel * Time.fixedDeltaTime;

        // Aplica un amortiment molt suau només a la component paral·lela
        Vector3 velocityNormal = Vector3.Dot(velocity, normal) * normal;
        Vector3 velocityParallel = velocity - velocityNormal;

        float frictionFactor = 0.995f; // Ajusta per més o menys amortiment

        velocityParallel *= frictionFactor;

        // Recombina velocitats
        velocity = velocityParallel + velocityNormal;

        // Tallar velocitat molt petita per evitar "vibracions"
        if (velocity.magnitude < 0.01f)
        {
            velocity = Vector3.zero;
        }
    }

    void HandleMovement()
    {
        transform.position += velocity * Time.fixedDeltaTime;

        // Manté la bola fora del pla (corrigeix penetració)
        if (currentSurface != null)
        {
            Vector3 normal = currentSurface.GetNormal().normalized;
            Plane plane = new Plane(normal, currentSurface.transform.position);

            float radius = 0.5f; // radi bola
            float dist = plane.GetDistanceToPoint(transform.position);

            if (dist < radius)
            {
                float penetration = radius - dist;
                transform.position += normal * penetration;
            }
        }
    }

    void CheckCollision()
    {
        float radius = 0.5f;

        isGrounded = false;

        foreach (var surface in CollisionManager.Instance.GetAllSurfaces())
        {
            Vector3 normal = surface.GetNormal().normalized;
            Plane plane = new Plane(normal, surface.transform.position);
            float dist = plane.GetDistanceToPoint(transform.position);

            if (dist < radius)
            {
                // Evitem que la bola entri dins el pla
                float penetration = radius - dist;
                transform.position += normal * penetration;

                // Reflexió o parada segons velocitat i direcció
                if (Vector3.Dot(velocity, normal) < -0.1f)
                {
                    velocity = Vector3.Reflect(velocity, normal) * .95f;
                }
                else if (velocity.magnitude < 0.1f)
                {
                    velocity = Vector3.zero;
                    isGrounded = true;
                    currentSurface = surface;
                }
            }
        }

        // Obstacles (similar)
        foreach (var obstacle in CollisionManager.Instance.GetAllObstacles())
        {
            Collider col = obstacle.GetComponent<Collider>();
            if (col == null) continue;

            Vector3 closestPoint = col.ClosestPoint(transform.position);
            Vector3 penetrationVector = transform.position - closestPoint;
            float dist = penetrationVector.magnitude;

            if (dist < radius)
            {
                // Corregim posició per fora de l'obstacle
                Vector3 correctionDir = penetrationVector.normalized;
                float penetration = radius - dist;
                transform.position += correctionDir * penetration;

                // Reflexió
                if (Vector3.Dot(velocity, correctionDir) < 0)
                {
                    velocity = Vector3.Reflect(velocity, correctionDir) * 0.8f;
                }
            }
        }
    }

    public void SetSurface(Surface surface)
    {
        currentSurface = surface;
        isGrounded = true;
    }

    public void ResetBall(Vector3 startPos)
    {
        transform.position = startPos;
        velocity = Vector3.zero;
        appliedForce = Vector3.zero;
    }
}