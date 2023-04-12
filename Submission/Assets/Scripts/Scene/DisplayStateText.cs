using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DisplayStateText : MonoBehaviour
{
    private void Awake()
    {
        string displayCount = Display.displays.Length == 1 ? "1 display" : "2 displays";
        GetComponent<TextMeshProUGUI>().text = $"Currently observing on\n{displayCount}.";
    }
}