using CodeMonkey.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementMouse : MonoBehaviour
{
    private bool isDialogBlocking;

    private void Awake()
    {
        EventBus.Subscribe<DialogBlockingEvent>(e => isDialogBlocking = e.status);
    }

    private void Start()
    {
        isDialogBlocking = false;
    }

    private void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame && !isDialogBlocking)
        {
            GetComponent<MovePositionDirect>().SetMovePosition(UtilsClass.GetMouseWorldPosition());
        }
    }
}