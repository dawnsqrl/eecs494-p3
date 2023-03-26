using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class HintDisplay : MonoBehaviour
{
    [SerializeField] private float marginWidth = 75;
    [SerializeField] private float lerpDuration = 0.5f;
    [SerializeField] private float lerpInterval = 0.4f;

    private RectTransform rectTransform;
    private TextMeshProUGUI content;
    private AnimationCurve dialogCurve;
    private AnimationCurve hintCurve;
    private bool isHintLerping;
    private bool isHintVisible;
    private bool doDismissHint;

    private void Awake()
    {
        EventBus.Subscribe<DisplayHintEvent>(_OnDisplayHint);
        EventBus.Subscribe<UpdateHintEvent>(_OnUpdateHint);
        EventBus.Subscribe<DismissHintEvent>(_ => doDismissHint = true);
        rectTransform = GetComponent<RectTransform>();
        content = GetComponentInChildren<TextMeshProUGUI>();
        dialogCurve = Resources.Load<AnimationCurveAsset>("Curves/DialogCurve");
        hintCurve = Resources.Load<AnimationCurveAsset>("Curves/HintCurve");
    }

    private void Start()
    {
        SetInitialHeight();
        content.text = "";
        content.alpha = 1;
        isHintLerping = false;
        isHintVisible = false;
        doDismissHint = false;
    }

    private void SetInitialHeight()
    {
        rectTransform.anchoredPosition = new Vector2(0, -rectTransform.rect.height);
    }

    private void _OnDisplayHint(DisplayHintEvent e)
    {
        if (isHintLerping || isHintVisible)
        {
            return;
        }

        rectTransform.sizeDelta = new Vector2(rectTransform.rect.width, e.height);
        content.text = e.text;
        SetInitialHeight();
        StartCoroutine(DisplayHint());
    }

    private void _OnUpdateHint(UpdateHintEvent e)
    {
        if (isHintLerping || !isHintVisible)
        {
            return;
        }

        StartCoroutine(UpdateHint(e.text));
    }

    private void Update()
    {
        if (doDismissHint)
        {
            if (!isHintLerping && isHintVisible)
            {
                StartCoroutine(DismissHint());
            }
            else
            {
                doDismissHint = false;
            }
        }
    }

    private IEnumerator DisplayHint()
    {
        isHintLerping = true;
        float initialHeight = -rectTransform.rect.height;
        float progress = 0;
        while (progress < 1)
        {
            rectTransform.anchoredPosition = new Vector2(
                0, Mathf.LerpUnclamped(initialHeight, marginWidth, hintCurve.Evaluate(progress))
            );
            progress += Time.deltaTime / lerpDuration;
            yield return null;
        }

        rectTransform.anchoredPosition = new Vector2(0, marginWidth);
        isHintVisible = true;
        isHintLerping = false;
    }

    private IEnumerator UpdateHint(string text)
    {
        isHintLerping = true;
        float progress = 0;
        while (progress < 1)
        {
            content.alpha = Mathf.Lerp(1, 0, dialogCurve.Evaluate(progress));
            progress += Time.deltaTime / (lerpInterval / 2);
            yield return null;
        }

        content.alpha = 0;
        content.text = text;
        progress = 1;
        while (progress > 0)
        {
            content.alpha = Mathf.Lerp(1, 0, dialogCurve.Evaluate(progress));
            progress -= Time.deltaTime / (lerpInterval / 2);
            yield return null;
        }

        content.alpha = 1;
        isHintLerping = false;
    }

    private IEnumerator DismissHint()
    {
        isHintLerping = true;
        doDismissHint = false;
        float initialHeight = -rectTransform.rect.height;
        float progress = 1;
        while (progress > 0)
        {
            rectTransform.anchoredPosition = new Vector2(
                0, Mathf.Lerp(initialHeight, marginWidth, dialogCurve.Evaluate(progress))
            );
            progress -= Time.deltaTime / lerpDuration;
            yield return null;
        }

        rectTransform.anchoredPosition = new Vector2(0, initialHeight);
        isHintVisible = false;
        isHintLerping = false;
    }
}