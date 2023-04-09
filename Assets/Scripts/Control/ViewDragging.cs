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

    [SerializeField] [Range(0f, 1f)] private float edgeTolerance = 0.001f;
    private Vector3 horizontalVelocity;
    private bool isDraggingEnabled, isBuilderTutorialActive = false;
    private Camera builderCam;

    Vector3 startDrag;

    [SerializeField] private float zoomMin = 0.5f;
    [SerializeField] private float zoomMax = 10.0f;

    // Speed of zooming
    private float zoomSpeed = 1.0f;

    bool openZoom = false, openDrag = false;

    private void Awake()
    {
        // set camera init position
        EventBus.Subscribe<AssignInitGrowthPositionEvent>(
            e =>
                transform.position = new Vector3(e.initPos.x, e.initPos.y, transform.position.z)
        );
        EventBus.Subscribe<DialogBlockingEvent>(e => isDraggingEnabled = !e.status);
        EventBus.Subscribe<StartBuilderTutorialEvent>(_ => isBuilderTutorialActive = true);
        EventBus.Subscribe<OpenZoomEvent>(_ => openZoom = true);
        EventBus.Subscribe<OpenDragEvent>(_ => openDrag = true);

        EventBus.Subscribe<CloseZoomEvent>(_ => openZoom = false);
        EventBus.Subscribe<CloseDragEvent>(_ => openDrag = false);
    }

    private void Start()
    {
        isDraggingEnabled = true;
        builderCam = GetComponent<Camera>();
        cameraTransform = GetComponent<Transform>();
    }

    private void Zoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // If the scroll value is not zero, adjust the camera size
        if (scroll != 0.0f)
        {
            // Get the current camera size
            float size = builderCam.orthographicSize;

            // Adjust the zoom based on the scroll value and speed
            float zoomDelta = scroll * zoomSpeed;
            size = Mathf.Clamp(size - zoomDelta, zoomMin, zoomMax);

            if (size >= 7.0f || size < 2.0f)
            {
                EventBus.Publish(new ZoomMaxEvent());
            }
                
            // Set the camera size to the new value
            builderCam.orthographicSize = size;
        }
    }

    public void setSize(float size)
    {
        builderCam.orthographicSize = size;
    }

    private void Update()
    {
        if ((isDraggingEnabled && GameProgressControl.isGameActive) || isBuilderTutorialActive)
        {
            if (isBuilderTutorialActive)
            {
                if (openZoom)
                    Zoom();
                if (openDrag)
                {
                    DragCamera();
                    //UpdateCameraPosition();
                    CheckMouseAtScreenEdge();
                    UpdateCameraPosition();
                }
            }
            else if (GameProgressControl.isGameActive)
            {
                Zoom();
                DragCamera();
                //UpdateCameraPosition();
                CheckMouseAtScreenEdge();
                UpdateCameraPosition();
            }
        }
    }


    private void DragCamera()
    {
        if (!Mouse.current.middleButton.isPressed)
        {
            return;
        }

        Plane plane = new Plane(new Vector3(0, 0, 1), Vector3.zero);
        Ray ray = builderCam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (plane.Raycast(ray, out float distance))
        {
            if (Mouse.current.middleButton.wasPressedThisFrame)
                startDrag = ray.GetPoint(distance);
            else
                targetPosition += startDrag - ray.GetPoint(distance);
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
        float detectionWidth = Screen.width;
        if (Display.displays.Length == 1)
        {
            detectionWidth /= 2;
        }

        //horizontal scrolling
        if (mousePosition.x < edgeTolerance * detectionWidth)
            moveDirection += -GetCameraRight();
        else if (mousePosition.x > (1f - edgeTolerance) * detectionWidth)
            moveDirection += GetCameraRight();

        //vertical scrolling
        if (mousePosition.y < edgeTolerance * Screen.height)
            moveDirection += -GetCameraForward();
        else if (mousePosition.y > (1f - edgeTolerance) * Screen.height)
            moveDirection += GetCameraForward();

        targetPosition += moveDirection;
    }

    private Vector3 ClampVector(Vector3 pos)
    {
        //TODO: may subjected to change if the display changes
        float new_x = Mathf.Clamp(pos.x, -5, 55);
        float new_y = Mathf.Clamp(pos.y, -5, 55);
        return new Vector3(new_x, new_y, pos.z);
    }

    private void UpdateCameraPosition()
    {
        //Vector3 topLeft = builderCam.ViewportToWorldPoint(new Vector3(0, 1, builderCam.nearClipPlane));
        //Vector3 topRight = builderCam.ViewportToWorldPoint(new Vector3(1, 1, builderCam.nearClipPlane));
        //Vector3 bottomLeft = builderCam.ViewportToWorldPoint(new Vector3(0, 0, builderCam.nearClipPlane));
        //Vector3 bottomRight = builderCam.ViewportToWorldPoint(new Vector3(1, 0, builderCam.nearClipPlane));
        //Debug.Log(bottomLeft);
        //Debug.Log(topRight);
        if (targetPosition.sqrMagnitude > 0.1f)
        {
            //create a ramp up or acceleration
            speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime * acceleration);
            transform.position += targetPosition * (speed * Time.deltaTime);
            transform.position = ClampVector(transform.position);
        }
        else
        {
            //create smooth slow down
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, Time.deltaTime * damping);
            transform.position += horizontalVelocity * Time.deltaTime;
            transform.position = ClampVector(transform.position);
        }

        //reset for next frame
        targetPosition = Vector3.zero;
    }
}