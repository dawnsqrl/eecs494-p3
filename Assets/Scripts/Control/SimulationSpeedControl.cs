using UnityEngine;

public class SimulationSpeedControl : MonoBehaviour
{
    [SerializeField] public int maxSimulationSpeedUnit = 20;

    private static int simulationSpeedUnit;
    private static bool isSimulationPaused;

    private void Awake()
    {
        EventBus.Subscribe<ScrollSimulationSpeedEvent>(_OnScrollSimulationSpeed);
        EventBus.Subscribe<ModifyPauseEvent>(e => isSimulationPaused = e.status);
    }

    private void Start()
    {
        simulationSpeedUnit = 10;
        isSimulationPaused = false;
    }

    private void _OnScrollSimulationSpeed(ScrollSimulationSpeedEvent e)
    {
        if (isSimulationPaused)
        {
            return;
        }

        if (e.direction)
        {
            if (simulationSpeedUnit < maxSimulationSpeedUnit)
            {
                simulationSpeedUnit += 1;
            }
        }
        else
        {
            if (simulationSpeedUnit > 1)
            {
                simulationSpeedUnit -= 1;
            }
        }
    }

    public static float GetSimulationSpeed()
    {
        return isSimulationPaused ? 0 : (float)simulationSpeedUnit / 10;
    }
}