using UnityEngine;
using UnityEngine.InputSystem;

public class DragDrop : MonoBehaviour
{
    [SerializeField] private GameObject GrowthController;

    private bool isDialogBlocking;
    private bool _mouseState;
    private GameObject target;
    public Vector3 screenSpace;
    public Vector3 offset;

    private void Awake()
    {
        EventBus.Subscribe<DialogBlockingEvent>(e => isDialogBlocking = e.status);
    }

    private void Start()
    {
        isDialogBlocking = false;
        _mouseState = false;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !(_mouseState || isDialogBlocking))
        {
            RaycastHit hitInfo;
            target = GetClickedObject(out hitInfo);
            if (target is not null)
            {
                Camera _camera = Camera.main;
                Vector3 targetPosition = target.transform.position;
                _mouseState = true;
                screenSpace = _camera.WorldToScreenPoint(targetPosition);
                offset = targetPosition - _camera.ScreenToWorldPoint(new Vector3(
                    Mouse.current.position.ReadValue().x,
                    Mouse.current.position.ReadValue().y,
                    screenSpace.z)
                );
            }
        }

        if (_mouseState && (Mouse.current.leftButton.wasReleasedThisFrame || isDialogBlocking))
        {
            _mouseState = false;
            //GrowthController.GetComponent<GrowthDemo>().setAim(Mathf.FloorToInt(transform.position.x),
                //Mathf.FloorToInt(transform.position.y));
        }

        if (_mouseState)
        {
            //keep track of the mouse position
            var curScreenSpace = new Vector3(
                Mouse.current.position.ReadValue().x,
                Mouse.current.position.ReadValue().y,
                screenSpace.z
            );

            //convert the screen mouse position to world point and adjust with offset
            var curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;

            //update the position of the object in the world
            target.transform.position = curPosition;
        }
    }


    GameObject GetClickedObject(out RaycastHit hit)
    {
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            target = hit.collider.gameObject;
            if (target.name != "Flag")
                target = null;
        }

        return target;
    }
}