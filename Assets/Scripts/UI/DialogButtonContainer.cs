using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogButtonContainer : MonoBehaviour
{
    [SerializeField] private float buttonHeight = 80;
    [SerializeField] private float buttonWidth = 200;
    [SerializeField] private float buttonSpacing = 40;

    private GameObject buttonTemplate;
    private GameObject buttonContainer;
    private AudioClip buttonAudio;

    private void Awake()
    {
        buttonTemplate = Resources.Load<GameObject>("Prefabs/Canvas/DialogButton");
        buttonAudio = Resources.Load<AudioClip>("Audio/MessagePopUp");
    }

    private void Start()
    {
        buttonContainer = null;
    }

    public void SetButton(Dictionary<string, Tuple<UnityAction, bool>> buttons)
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
            foreach (KeyValuePair<string, Tuple<UnityAction, bool>> button in buttons)
            {
                GameObject thisButton = Instantiate(buttonTemplate, buttonContainer.transform);
                thisButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                    (index - 1) * (buttonWidth + buttonSpacing) - offset, 0
                );
                thisButton.GetComponentInChildren<TextMeshProUGUI>().text = button.Key;
                Button.ButtonClickedEvent buttonClickedEvent = thisButton.GetComponent<Button>().onClick;
                buttonClickedEvent.AddListener(() => AudioSource.PlayClipAtPoint(buttonAudio, AudioListenerManager.audioListenerPos, 0.5f));
                if (button.Value is not null)
                {
                    if (button.Value.Item1 is not null)
                    {
                        buttonClickedEvent.AddListener(button.Value.Item1);
                    }

                    if (button.Value.Item2)
                    {
                        buttonClickedEvent.AddListener(() => EventBus.Publish(new DismissDialogEvent()));
                    }
                }

                index++;
            }
        }
        // else
        // {
        //     GameObject thisButton = Instantiate(buttonTemplate, buttonContainer.transform);
        //     thisButton.GetComponentInChildren<TextMeshProUGUI>().text = StringPool.defaultDialogButtonText;
        //     thisButton.GetComponent<Button>().onClick.AddListener(_DismissAction);
        // }
    }
}