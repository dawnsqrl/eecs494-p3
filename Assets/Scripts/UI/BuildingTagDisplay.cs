using UnityEngine;

public class BuildingTagDisplay : MonoBehaviour
{
    private GameObject banner;
    private bool isBuilderTutorialActive;
    private bool isDialogBlocking;

    private void Awake()
    {
        EventBus.Subscribe<StartBuilderTutorialEvent>(_ => isBuilderTutorialActive = true);
        EventBus.Subscribe<EndBuilderTutorialEvent>(_ => isBuilderTutorialActive = false);
        EventBus.Subscribe<DialogBlockingEvent>(e => isDialogBlocking = e.status);
        banner = GetComponentInChildren<BannerDisplay>().gameObject;
    }

    private void Start()
    {
        banner.SetActive(false);
    }

    public void OnHoverEnter()
    {
        if (isDialogBlocking)
        {
            return;
        }

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