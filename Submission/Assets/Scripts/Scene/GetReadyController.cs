using TMPro;
using UnityEngine;

public class GetReadyController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mousePrompt;
    [SerializeField] private TextMeshProUGUI keyboardPrompt;

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
        mousePrompt.text = "[LClick] to get ready!";
        keyboardPrompt.text = "[Enter] to get ready!";
    }

    private void Update()
    {
        if (!hasMushroomConfirmed && playerActions.ConfirmMushroomControl.WasPressedThisFrame())
        {
            hasMushroomConfirmed = true;
            mousePrompt.text = "Mushroom is ready!";
            ColorUtility.TryParseHtmlString("#f3be09", out Color color);
            mousePrompt.color = color;
        }

        if (!hasSnailConfirmed && playerActions.ConfirmSnailControl.WasPressedThisFrame())
        {
            hasSnailConfirmed = true;
            keyboardPrompt.text = "Snail is ready!";
            ColorUtility.TryParseHtmlString("#a1dfc1", out Color color);
            keyboardPrompt.color = color;
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