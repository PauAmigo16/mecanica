using UnityEngine;
using TMPro;

public class SimulationControl : MonoBehaviour
{
    public GravityManager gravityManager;
    public TMP_Text buttonLabel;
    public Planet[] allPlanets;

    private bool simulating = true;

    public void ToggleSimulation()
    {
        simulating = !simulating;
        gravityManager.enabled = simulating;
        buttonLabel.text = simulating ? "Pause" : "Continue";
    }

    public void ResetSimulation()
    {
        foreach (Planet p in allPlanets)
        {
            p.ResetToDefaults(gravityManager.timeStep * gravityManager.timeScale);
        }

        if (!simulating)
        {
            ToggleSimulation();
        }
    }
}