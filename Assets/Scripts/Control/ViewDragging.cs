using UnityEngine;
using UnityEngine.InputSystem;

public class ViewDragging : MonoBehaviour
{
    //private CameraControls cameraActions;
    private InputAction movement;
    private Transform cameraTransform;

    private Vector3 targetPosition;
    private Vector3 initPosition;
    [SerializeField] private float maxSpeed = 5f;
    private float speed;

    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float damping = 15f;

    [SerializeField] [Range(0f, 1f)] private float edgeTolerance = 0.05f;
    private Vector3 horizontalVelocity;
    private bool isDraggingEnabled;

    private void Awake()
    {
        // set camera init position
        EventBus.Subscribe<AssignInitGrowthPositionEvent>(
            e =>
                transform.position = new Vector3(e.initPos.x, e.initPos.y, transform.position.z)
        );
        EventBus.Subscribe<ModifyPauseEvent>(e => isDraggingEnabled = !e.status);
        cameraTransform = GetComponent<Transform>();
    }

    private void Start()
    {
        isDraggingEnabled = true;
    }

    private void Update()
    {
        if (isDraggingEnabled)
        {
            CheckMouseAtScreenEdge();
            UpdateCameraPosition();
        }
    }

    //gets the horizontal forward vector of the camera
    private Vector3 GetCameraForward()
    {
        Vector3 forward = cameraTransform.up;
        forward.z = 0f;
        return forward;
    }

    //gets the horizontal right vector of the camera
    private Vector3 GetCameraRight()
    {
        Vector3 right = cameraTransform.right;
        right.z = 0f;
        return right;
    }

    private void CheckMouseAtScreenEdge()
    {
        //mouse position is in pixels
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 moveDirection = Vector3.zero;

        //horizontal scrolling
        if (mousePosition.x < edgeTolerance * Screen.width)
            moveDirection += -GetCameraRight();
        else if (mousePosition.x > (1f - edgeTolerance) * Screen.width)
            moveDirection += GetCameraRight();

        //vertical scrolling
        if (mousePosition.y < edgeTolerance * Screen.height)
            moveDirection += -GetCameraForward();
        else if (mousePosition.y > (1f - edgeTolerance) * Screen.height)
            moveDirection += GetCameraForward();

        targetPosition += moveDirection;
    }

    private void UpdateCameraPosition()
    {
        if (targetPosition.sqrMagnitude > 0.1f)
        {
            //create a ramp up or acceleration
            speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime * acceleration);
            transform.position += targetPosition * (speed * Time.deltaTime);
        }
        else
        {
            //create smooth slow down
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, Time.deltaTime * damping);
            transform.position += horizontalVelocity * Time.deltaTime;
        }

        //reset for next frame
        targetPosition = Vector3.zero;
    }
}