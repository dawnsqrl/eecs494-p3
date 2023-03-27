using UnityEngine;

public class BuildingTagDisplay : MonoBehaviour
{
    private GameObject banner;
    private bool isBuilderTutorialActive;

    private void Awake()
    {
        EventBus.Subscribe<StartBuilderTutorialEvent>(_ => isBuilderTutorialActive = true);
        EventBus.Subscribe<EndBuilderTutorialEvent>(_ => isBuilderTutorialActive = false);
        banner = GetComponentInChildren<BannerDisplay>().gameObject;
    }

    private void Start()
    {
        banner.SetActive(false);
        isBuilderTutorialActive = false;
    }

    public void OnHoverEnter()
    {
        if (GameProgressControl.isGameActive || isBuilderTutorialActive)
        {
            banner.SetActive(true);
        }
    }

    public void OnHoverExit()
    {
        banner.SetActive(false);
    }
}