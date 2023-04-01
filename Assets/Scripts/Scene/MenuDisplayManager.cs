using UnityEngine;

public class MenuDisplayManager : MonoBehaviour
{
    [SerializeField] private Camera mouseCamera;
    [SerializeField] private Canvas mouseCanvas;
    [SerializeField] private Camera keyboardCamera;
    [SerializeField] private Canvas keyboardCanvas;

    private readonly Rect rectFull = new Rect(0, 0, 1, 1);

    private void Awake()
    {
        if (Display.displays.Length > 1)
        {
            mouseCamera.targetDisplay = 1;
            mouseCamera.rect = rectFull;
            mouseCanvas.targetDisplay = 1;
            keyboardCamera.targetDisplay = 0;
            keyboardCamera.rect = rectFull;
            keyboardCanvas.targetDisplay = 0;
        }
        else
        {
            mouseCamera.targetDisplay = 0;
            mouseCamera.rect = rectFull;
            mouseCanvas.targetDisplay = 0;
            keyboardCamera.enabled = false;
            keyboardCanvas.enabled = false;
        }
    }
}