using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(RectTransform))]
public class CursorUpdater : MonoBehaviour
{
    private Image cursor;
    private RectTransform rectTransform;
    private Sprite defaultCursorSprite;
    private float size;

    private void Awake()
    {
        EventBus.Subscribe<UpdateCursorEvent>(_OnUpdateCursor);
        cursor = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        defaultCursorSprite = Resources.Load<Sprite>("Sprites/Theme/Cursor");
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        cursor.sprite = defaultCursorSprite;
        cursor.color = new Color(1, 1, 1, 1);
        size = 64;
        rectTransform.sizeDelta = new Vector2(1, 1) * size;
    }

    private void Update()
    {
        rectTransform.anchoredPosition = Mouse.current.position.ReadValue();
        if (Keyboard.current.minusKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if (Keyboard.current.equalsKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    private void _OnUpdateCursor(UpdateCursorEvent e)
    {
        cursor.sprite = e.sprite ?? defaultCursorSprite;
        cursor.color = new Color(1, 1, 1, e.alpha);
        size = e.size;
        rectTransform.sizeDelta = new Vector2(1, 1) * size;
    }
}