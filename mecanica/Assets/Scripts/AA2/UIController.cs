using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    [Header("Referències")]
    public BallPhysics ball;

    public List<Surface> surfaces = new List<Surface>();

    public Slider massSlider;
    public Slider frictionSlider;
    public Slider velocityXSlider;
    public Slider velocityYSlider;

    public Button resetButton;  // Referència al botó de reinici

    private Vector3 initialPosition;

    void Start()
    {
        if (ball != null)
        {
            massSlider.value = ball.mass;
            initialPosition = ball.transform.position;
        }

        if (surfaces.Count > 0)
            frictionSlider.value = surfaces[0].friction;

        velocityXSlider.value = 0f;
        velocityYSlider.value = 0f;

        if (resetButton != null)
            resetButton.onClick.AddListener(ResetBallPosition);
    }

    public void OnMassChanged()
    {
        if (ball != null)
            ball.mass = massSlider.value;
    }

    public void OnFrictionChanged()
    {
        foreach (var surface in surfaces)
        {
            surface.friction = frictionSlider.value;
        }
    }

    public void OnVelocityXChanged()
    {
        if (ball != null)
            ball.velocity.x = velocityXSlider.value;
    }

    public void OnVelocityYChanged()
    {
        if (ball != null)
            ball.velocity.y = velocityYSlider.value;
    }

    private void ResetBallPosition()
    {
        if (ball != null)
        {
            ball.ResetBall(initialPosition);
            ball.velocity = new Vector3(velocityXSlider.value, velocityYSlider.value, 0f);
        }
    }
}
