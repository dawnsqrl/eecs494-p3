using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
public class PopupDisplay : MonoBehaviour
{
    [SerializeField] private float buttonSize = 1.5f;
    [SerializeField] private float buttonSpacing = 0.5f;
    [SerializeField] private float lerpDuration = 0.05f;

    private GameObject buttonTemplate;
    private RectTransform rectTransform;
    private Image image;
    private Material material;
    private Camera targetCamera;
    private bool isMouseOver;
    private readonly int color = Shader.PropertyToID("_Color");

    private void Awake()
    {
        buttonTemplate = Resources.Load<GameObject>("Prefabs/Canvas/PopupButton");
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        material = new Material(image.material);
        image.material = material;
        targetCamera = transform.parent.GetComponent<Canvas>().worldCamera;
    }

    public void InitializePopup(DisplayPopupEvent e)
    {
        material.SetColor(color, e.color);
        int count = e.buttons.Count;
        int rowCount = Mathf.CeilToInt((float)count / 2);
        int columnCount = count == 1 ? 1 : 2;
        float width = (buttonSize + buttonSpacing) * columnCount + buttonSpacing;
        float height = (buttonSize + buttonSpacing) * rowCount + buttonSpacing;
        rectTransform.sizeDelta = new Vector2(width, height);
        int index = 1;
        foreach (KeyValuePair<string, UnityAction> button in e.buttons)
        {
            GameObject thisButton = Instantiate(buttonTemplate, transform);
            int rowIndex = Mathf.CeilToInt((float)index / 2) - 1;
            int columnIndex = (index - 1) % 2;
            RectTransform thisRectTransform = thisButton.GetComponent<RectTransform>();
            thisRectTransform.sizeDelta = new Vector2(buttonSize, buttonSize);
            thisRectTransform.anchoredPosition = count == 1
                ? Vector2.zero
                : new Vector2(
                    (columnIndex + 1) * (buttonSize + buttonSpacing) - buttonSize / 2,
                    -((rowIndex + 1) * (buttonSize + buttonSpacing) - buttonSize / 2)
                );

            if (button.Key.Length > 0)
            {
                Sprite sprite = Resources.Load<Sprite>(button.Key);
                if (sprite is not null)
                {
                    thisButton.GetComponentInChildren<PopupButtonSprite>().SetSprite(sprite);
                }
            }

            if (button.Value is not null)
            {
                thisButton.GetComponent<Button>().onClick.AddListener(button.Value);
            }

            index++;
        }

        Vector2 popupPosition = e.isWorldPosition ? e.position : targetCamera.ScreenToWorldPoint(e.position);
        popupPosition.x -= rectTransform.rect.width / 2 + e.offset;
        rectTransform.anchoredPosition = popupPosition;
    }

    private void Start()
    {
        isMouseOver = false;
    }

    public void OnPointerEnter()
    {
        isMouseOver = true;
    }

    public void OnPointerExit()
    {
        isMouseOver = false;
    }

    public bool GetMouseOverState()
    {
        return isMouseOver;
    }
}