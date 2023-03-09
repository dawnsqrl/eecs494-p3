using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DialogContent : MonoBehaviour
{
    private TextMeshProUGUI content;

    private void Awake()
    {
        content = GetComponent<TextMeshProUGUI>();
    }

    public void SetContent(string _content)
    {
        content.text = _content;
    }
}