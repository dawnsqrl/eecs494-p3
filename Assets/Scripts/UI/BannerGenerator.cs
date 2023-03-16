using UnityEngine;

public class BannerGenerator : MonoBehaviour
{
    private GameObject bannerCanvasTemplate;

    private void Awake()
    {
        EventBus.Subscribe<DisplayBannerEvent>(_OnDisplayBanner);
        bannerCanvasTemplate = Resources.Load<GameObject>("Prefabs/Canvas/BannerCanvas");
    }

    private void _OnDisplayBanner(DisplayBannerEvent e)
    {
        GameObject bannerCanvas = Instantiate(bannerCanvasTemplate, e.parent);
        bannerCanvas.GetComponentInChildren<BannerDisplay>().InitializeBanner(e);
    }
}