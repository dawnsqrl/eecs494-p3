using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class BannerDisplay : MonoBehaviour
{
    [SerializeField] private float fontSize = 3;
    [SerializeField] private float fontPadding = 2;
    [SerializeField] private float lerpDuration = 0.05f;

    private RectTransform rectTransform;
    private Image image;
    private Material material;
    private TextMeshProUGUI content;
    private readonly int color = Shader.PropertyToID("_Color");

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        material = new Material(image.material);
        image.material = material;
        content = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void InitializeBanner(DisplayBannerEvent e)
    {
        material.SetColor(color, e.color);
        content.fontSize = fontSize;
        Vector2 textSize = content.GetPreferredValues(e.text);
        rectTransform.sizeDelta = new Vector2(
            textSize.x + fontPadding, textSize.y + fontPadding
        );
        content.text = e.text;
        Vector2 anchoredPosition = rectTransform.anchoredPosition;
        rectTransform.anchoredPosition = new Vector2(
            anchoredPosition.x, anchoredPosition.y + e.offset
        );
    }
}