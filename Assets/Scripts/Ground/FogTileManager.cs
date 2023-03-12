using UnityEngine;

public class FogTileManager : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (!PauseControl.isPaused)
        {
            gameObject.SetActive(false);
        }
    }
}