using UnityEngine;
using UnityEngine.InputSystem;

public class PopupGenerator : MonoBehaviour
{
    private GameObject popupDisplayTemplate;
    private GameObject popupDisplay;
    private PopupDisplay popupDisplayComponent;

    private void Awake()
    {
        EventBus.Subscribe<DisplayPopupEvent>(_OnDisplayPopup);
        popupDisplayTemplate = Resources.Load<GameObject>("Prefabs/Canvas/PopupDisplay");
    }

    private void _OnDisplayPopup(DisplayPopupEvent e)
    {
        if (popupDisplay is not null)
        {
            DestroyPopup();
        }

        popupDisplay = Instantiate(popupDisplayTemplate, transform);
        popupDisplayComponent = popupDisplay.GetComponent<PopupDisplay>();
        popupDisplayComponent.InitializePopup(e);
    }

    private void DestroyPopup()
    {
        // fade out and destroy
        Destroy(popupDisplay);
        popupDisplay = null;
    }

    private void Start()
    {
        popupDisplay = null;
    }

    private void Update()
    {
        if (popupDisplayComponent is null)
        {
            return;
        }

        if ((Mouse.current.leftButton.wasPressedThisFrame && !popupDisplayComponent.GetMouseOverState())
            || Mouse.current.rightButton.wasPressedThisFrame)
        {
            DestroyPopup();
        }
    }
}