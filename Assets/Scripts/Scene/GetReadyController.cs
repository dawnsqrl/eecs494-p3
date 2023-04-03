using UnityEngine;

public class GetReadyController : MonoBehaviour
{
    private Sprite mouseGameImage;
    private Sprite keyboardGameImage;
    private Controls controls;
    private Controls.PlayerActions playerActions;
    private bool hasMushroomConfirmed;
    private bool hasSnailConfirmed;

    private void Awake()
    {
        mouseGameImage = Resources.Load<Sprite>("Sprites/Background/MouseGame");
        keyboardGameImage = Resources.Load<Sprite>("Sprites/Background/KeyboardGame");
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

    private void Start()
    {
        hasMushroomConfirmed = false;
        hasSnailConfirmed = false;
    }

    private void Update()
    {
        if (!hasMushroomConfirmed && playerActions.ConfirmMushroomControl.WasPressedThisFrame())
        {
            hasMushroomConfirmed = true;
        }

        if (!hasSnailConfirmed && playerActions.ConfirmSnailControl.WasPressedThisFrame())
        {
            hasSnailConfirmed = true;
        }

        if (hasMushroomConfirmed && hasSnailConfirmed)
        {
            SceneState.SetTransition(
                1, 0, "MainTutorial", mouseGameImage, keyboardGameImage
            );
            EventBus.Publish(new TransitSceneEvent());
        }
    }
}