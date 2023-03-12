using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class SimulationSpeedDisplay : MonoBehaviour
{
    private TextMeshProUGUI display;

    private void Awake()
    {
        display = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        display.text = $"Game speed: {SimulationSpeedControl.GetSimulationSpeed()}x";
    }
}