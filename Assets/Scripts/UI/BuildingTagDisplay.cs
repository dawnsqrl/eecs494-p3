using UnityEngine;

public class BuildingTagDisplay : MonoBehaviour
{
    private GameObject bannerPositive;
    private GameObject bannerNegative;
    private bool isBuilderTutorialActive;
    private bool isDialogBlocking;

    private void Awake()
    {
        EventBus.Subscribe<StartBuilderTutorialEvent>(_ => isBuilderTutorialActive = true);
        EventBus.Subscribe<EndBuilderTutorialEvent>(_ => isBuilderTutorialActive = false);
        EventBus.Subscribe<DialogBlockingEvent>(e => isDialogBlocking = e.status);
        bannerPositive = GetComponentInChildren<BuildingTagPositive>().gameObject;
        bannerNegative = GetComponentInChildren<BuildingTagNegative>().gameObject;
    }

    private void Start()
    {
        OnHoverExit();
    }

    public void OnHoverEnter()
    {
        if (isDialogBlocking)
        {
            return;
        }

        if (GameProgressControl.isGameActive || isBuilderTutorialActive)
        {
            bannerPositive.SetActive(true);
            bannerNegative.SetActive(false);
        }
    }

    public void OnHoverExit()
    {
        bannerPositive.SetActive(false);
        bannerNegative.SetActive(true);
    }
}