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
        mouseCamera.targetDisplay = 0;
        mouseCamera.rect = rectFull;
        mouseCanvas.targetDisplay = 0;
        if (Display.displays.Length > 1)
        {
            Display.displays[1].Activate();
            keyboardCamera.targetDisplay = 1;
            keyboardCamera.rect = rectFull;
            keyboardCanvas.targetDisplay = 1;
        }
        else
        {
            keyboardCamera.enabled = false;
            keyboardCanvas.enabled = false;
        }
    }
}