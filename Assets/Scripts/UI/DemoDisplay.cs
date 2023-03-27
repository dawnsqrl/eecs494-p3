using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DemoDisplay : MonoBehaviour
{
    private TextMeshProUGUI display;

    private void Awake()
    {
        EventBus.Subscribe<ToggleDemoEvent>(_ => display.enabled = !display.enabled);
        display = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        display.enabled = false;
    }
}