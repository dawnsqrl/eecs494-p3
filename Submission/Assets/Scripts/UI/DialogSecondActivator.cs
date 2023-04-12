using UnityEngine;

public class DialogSecondActivator : MonoBehaviour
{
    private void Awake()
    {
        if (Display.displays.Length == 1)
        {
            gameObject.SetActive(false);
        }
    }
}