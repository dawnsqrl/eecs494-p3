using UnityEngine;
using UnityEngine.EventSystems;

public class FogTileManager : MonoBehaviour
{
    private bool isClickActive;

    private void Awake()
    {
        EventBus.Subscribe<ModifyPauseEvent>(e => isClickActive = !e.status);
    }

    private void Start()
    {
        isClickActive = true;
    }

    private void OnMouseDown()
    {
        if (isClickActive && !EventSystem.current.IsPointerOverGameObject())
        {
            gameObject.SetActive(false);
        }
    }
}