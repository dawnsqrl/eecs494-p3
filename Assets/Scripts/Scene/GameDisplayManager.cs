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
        mouseCamera.targetDisplay = 0;
        dividerCanvas.SetActive(false);
        //Display.displays[1].Activate();
        //keyboardCamera.targetDisplay = 0;
        mouseCamera.rect = rectFull;
        keyboardCamera.rect = new Rect(0, 0, 0, 0);
    }
}