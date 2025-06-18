using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PlanetUIController : MonoBehaviour
{
    public Planet targetPlanet;
    public GravityManager gravityManager;

    public Slider massSlider;
    public TMP_Text massLabel;

    public Slider speedSlider;
    public TMP_Text speedLabel;

    public Slider distanceSlider;
    public TMP_Text distanceLabel;

    public Slider timeScaleSlider;
    public TMP_Text timeScaleLabel;

    public TMP_Dropdown planetDropdown;
    public Planet[] planets;

    void Start()
    {
        massSlider.value = targetPlanet.mass;
        speedSlider.value = targetPlanet.initialVelocity.magnitude;
        distanceSlider.value = targetPlanet.transform.position.magnitude;
        timeScaleSlider.value = gravityManager.timeScale;

        planetDropdown.ClearOptions();
        List<string> names = new List<string>();
        foreach (Planet p in planets)
            names.Add(p.gameObject.name);
        planetDropdown.AddOptions(names);

        targetPlanet = planets[0];
        planetDropdown.onValueChanged.AddListener(OnDropdownChanged);

        UpdateSliders();
    }

    void OnDropdownChanged(int index)
    {
        targetPlanet = planets[index];
        UpdateSliders();
    }

    void UpdateSliders()
    {
        if (targetPlanet == null) return;
        massSlider.value = targetPlanet.mass;
        speedSlider.value = targetPlanet.initialVelocity.magnitude;
        distanceSlider.value = targetPlanet.transform.position.magnitude;
    }

    void Update()
    {
        if (targetPlanet == null || gravityManager == null) return;

        targetPlanet.mass = massSlider.value;
        massLabel.text = targetPlanet.mass.ToString("0.00e0");

        Vector3 dir = Vector3.Cross((targetPlanet.transform.position - Vector3.zero).normalized, Vector3.up).normalized;
        targetPlanet.initialVelocity = dir * speedSlider.value;
        speedLabel.text = speedSlider.value.ToString("0.00");

        Vector3 direction = (targetPlanet.transform.position - Vector3.zero).normalized;
        targetPlanet.transform.position = direction * distanceSlider.value;
        distanceLabel.text = distanceSlider.value.ToString("0.00");

        gravityManager.timeScale = timeScaleSlider.value;
        timeScaleLabel.text = timeScaleSlider.value.ToString("0.0");
    }

    public void ApplyVelocity()
    {
        if (targetPlanet != null)
        {
            targetPlanet.ReApplyVelocity(gravityManager.timeStep * gravityManager.timeScale);
        }
    }
}