using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera movingCamera;
    [SerializeField] private GameObject dividerCanvas;

    private void Awake()
    {
        if (Display.displays.Length > 1)
        {
            dividerCanvas.SetActive(false);
            Display.displays[1].Activate();
            movingCamera.targetDisplay = 1;
            mainCamera.rect = new Rect(0, 0, 1, 1);
            movingCamera.rect = new Rect(0, 0, 1, 1);
        }
    }
}