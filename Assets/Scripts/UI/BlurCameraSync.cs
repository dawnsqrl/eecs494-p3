using UnityEngine;

[RequireComponent(typeof(Camera))]
public class BlurCameraSync : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;

    private Camera blurCamera;

    private void Awake()
    {
        blurCamera = GetComponent<Camera>();
    }

    private void Start()
    {
        blurCamera.cullingMask = targetCamera.cullingMask & ~(1 << LayerMask.NameToLayer("UI"));
        blurCamera.nearClipPlane = targetCamera.nearClipPlane;
        blurCamera.farClipPlane = targetCamera.farClipPlane;
        blurCamera.targetDisplay = targetCamera.targetDisplay;
    }

    private void Update()
    {
        blurCamera.transform.position = targetCamera.transform.position;
        blurCamera.orthographicSize = targetCamera.orthographicSize;
    }
}