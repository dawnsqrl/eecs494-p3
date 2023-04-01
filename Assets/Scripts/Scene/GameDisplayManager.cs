using UnityEngine;

public class GameDisplayManager : MonoBehaviour
{
    [SerializeField] private Camera mouseCamera;
    [SerializeField] private Camera keyboardCamera;
    [SerializeField] private GameObject dividerCanvas;

    private readonly Rect rectFull = new Rect(0, 0, 1, 1);
    private readonly Rect rectLeft = new Rect(0, 0, 0.5f, 1);
    private readonly Rect rectRight = new Rect(0.5f, 0, 0.5f, 1);

    private void Awake()
    {
        if (Display.displays.Length > 1)
        {
            dividerCanvas.SetActive(false);
            Display.displays[1].Activate();
            mouseCamera.targetDisplay = 1;
            keyboardCamera.targetDisplay = 0;
            mouseCamera.rect = rectFull;
            keyboardCamera.rect = rectFull;
        }
        else
        {
            dividerCanvas.SetActive(true);
            mouseCamera.targetDisplay = 0;
            keyboardCamera.targetDisplay = 0;
            mouseCamera.rect = rectLeft;
            keyboardCamera.rect = rectRight;
        }
    }
}