using UnityEngine;

public class InputInterface : MonoBehaviour
{
    private Controls controls;
    private Controls.PlayerActions playerActions;

    private void Awake()
    {
        controls = new Controls();
        playerActions = controls.Player;
    }
    
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    
    private void Update()
    {
        if (playerActions.TogglePause.WasPressedThisFrame())
        {
            EventBus.Publish(new TogglePauseEvent());
        }
    }
}