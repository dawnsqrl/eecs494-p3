using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DialogTitle : MonoBehaviour
{
    private TextMeshProUGUI title;

    private void Awake()
    {
        title = GetComponent<TextMeshProUGUI>();
    }

    public void SetTitle(string _title)
    {
        title.text = _title;
    }
}