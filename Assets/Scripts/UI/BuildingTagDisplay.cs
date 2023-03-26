using UnityEngine;

public class BuildingTagDisplay : MonoBehaviour
{
    private GameObject banner;

    private void Awake()
    {
        banner = GetComponentInChildren<BannerDisplay>().gameObject;
    }

    private void Start()
    {
        banner.SetActive(false);
    }

    public void OnHoverEnter()
    {
        if (GameProgressControl.isGameActive)
        {
            banner.SetActive(true);
        }
    }

    public void OnHoverExit()
    {
        banner.SetActive(false);
    }
}