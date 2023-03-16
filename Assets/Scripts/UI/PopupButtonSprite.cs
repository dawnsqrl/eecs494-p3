using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PopupButtonSprite : MonoBehaviour
{
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }
}