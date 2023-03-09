using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogButtonContainer : MonoBehaviour
{
    [SerializeField] private float buttonHeight = 50;
    [SerializeField] private float buttonWidth = 200;
    [SerializeField] private float buttonSpacing = 20;

    private GameObject buttonTemplate;
    private GameObject buttonContainer;

    private void Awake()
    {
        buttonTemplate = Resources.Load<GameObject>("Prefabs/Canvas/DialogButton");
    }

    private void Start()
    {
        buttonContainer = null;
    }

    public void SetButton(Dictionary<string, UnityAction> buttons)
    {
        if (buttonContainer is not null)
        {
            Destroy(buttonContainer);
        }

        buttonContainer = new GameObject("This thing makes it easier to destroy all buttons", typeof(RectTransform));
        RectTransform rectTransform = buttonContainer.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(0, buttonHeight);
        rectTransform.SetParent(transform, false);
        int count = buttons.Count;
        if (count > 0)
        {
            float offset = (count - 1) * (buttonWidth + buttonSpacing) / 2;
            int index = 1;
            foreach (KeyValuePair<string, UnityAction> button in buttons)
            {
                GameObject thisButton = Instantiate(buttonTemplate, buttonContainer.transform);
                thisButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                    (index - 1) * (buttonWidth + buttonSpacing) - offset, 0
                );
                thisButton.GetComponentInChildren<TextMeshProUGUI>().text = button.Key;
                Button.ButtonClickedEvent buttonClickedEvent = thisButton.GetComponent<Button>().onClick;
                buttonClickedEvent.AddListener(_DefaultAction);
                if (button.Value is not null)
                {
                    buttonClickedEvent.AddListener(button.Value);
                }

                index++;
            }
        }
        else
        {
            GameObject thisButton = Instantiate(buttonTemplate, buttonContainer.transform);
            thisButton.GetComponentInChildren<TextMeshProUGUI>().text = StringPool.defaultDialogButtonText;
            thisButton.GetComponent<Button>().onClick.AddListener(_DefaultAction);
        }
    }

    private void _DefaultAction()
    {
        EventBus.Publish(new DismissDialogEvent());
    }
}