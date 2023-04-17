using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
public class HintDisplay : MonoBehaviour
{
    [SerializeField] private int index;
    [SerializeField] private float marginWidth = 75;
    [SerializeField] private float lerpDuration = 0.5f;
    [SerializeField] private float lerpInterval = 0.4f;

    private RectTransform rectTransform;
    private Image image;
    private TextMeshProUGUI content;
    private AnimationCurve dialogCurve;
    private AnimationCurve hintCurve;
    private AudioClip pageAudio;
    private bool isHintLerping;
    private bool isHintVisible;
    private bool doDismissHint;
    private float currentMarginWidth;

    private void Awake()
    {
        EventBus.Subscribe<DisplayHintEvent>(_OnDisplayHint);
        EventBus.Subscribe<UpdateHintEvent>(_OnUpdateHint);
        EventBus.Subscribe<DismissHintEvent>(e =>
        {
            if (e.index == index)
            {
                doDismissHint = true;
            }
        });
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        content = GetComponentInChildren<TextMeshProUGUI>();
        dialogCurve = Resources.Load<AnimationCurveAsset>("Curves/DialogCurve");
        hintCurve = Resources.Load<AnimationCurveAsset>("Curves/HintCurve");
        pageAudio = Resources.Load<AudioClip>("Audio/NotePressed");
    }

    private void Start()
    {
        SetInitialHeight();
        content.text = "";
        content.alpha = 1;
        isHintLerping = false;
        isHintVisible = false;
        doDismissHint = false;
        currentMarginWidth = marginWidth;
    }

    private void SetInitialHeight()
    {
        rectTransform.anchoredPosition = new Vector2(0, -rectTransform.rect.height);
    }

    private void _OnDisplayHint(DisplayHintEvent e)
    {
        if (e.index != index || isHintLerping || isHintVisible)
        {
            return;
        }

        rectTransform.offsetMin = new Vector2(marginWidth, rectTransform.offsetMin.y);
        rectTransform.offsetMax = new Vector2(-marginWidth, rectTransform.offsetMax.y);
        content.text = e.text;
        SetInitialHeight();
        currentMarginWidth = e.margin < 0 ? marginWidth : e.margin;
        StartCoroutine(DisplayHint(currentMarginWidth));
    }

    private void _OnUpdateHint(UpdateHintEvent e)
    {
        if (e.index != index || isHintLerping || !isHintVisible)
        {
            return;
        }

        float newMarginWidth = e.margin < 0 ? marginWidth : e.margin;
        if (Mathf.Approximately(currentMarginWidth, newMarginWidth))
        {
            StartCoroutine(UpdateHint(e.text));
        }
        else
        {
            currentMarginWidth = newMarginWidth;
            StartCoroutine(DisplaceHint(e.text, currentMarginWidth));
        }
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

    private IEnumerator DisplayHint(float newMarginWidth)
    {
        AudioSource.PlayClipAtPoint(pageAudio, transform.position);
        isHintLerping = true;
        float initialHeight = -rectTransform.rect.height;
        float progress = 0;
        while (progress < 1)
        {
            rectTransform.anchoredPosition = new Vector2(
                0, Mathf.LerpUnclamped(initialHeight, newMarginWidth, hintCurve.Evaluate(progress))
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
        AudioSource.PlayClipAtPoint(pageAudio, transform.position);
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

    private IEnumerator DisplaceHint(string text, float newMarginWidth)
    {
        AudioSource.PlayClipAtPoint(pageAudio, transform.position);
        isHintLerping = true;
        Color originalColor = image.color;
        Color transitionColor = originalColor;
        transitionColor.a = 0;
        float progress = 0;
        while (progress < 1)
        {
            float curveValue = dialogCurve.Evaluate(progress);
            image.color = Color.Lerp(originalColor, transitionColor, curveValue);
            content.alpha = Mathf.Lerp(1, 0, curveValue);
            progress += Time.deltaTime / (lerpInterval / 2);
            yield return null;
        }

        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, newMarginWidth);
        image.color = transitionColor;
        content.alpha = 0;
        content.text = text;
        progress = 1;
        while (progress > 0)
        {
            float curveValue = dialogCurve.Evaluate(progress);
            image.color = Color.Lerp(originalColor, transitionColor, curveValue);
            content.alpha = Mathf.Lerp(1, 0, curveValue);
            progress -= Time.deltaTime / (lerpInterval / 2);
            yield return null;
        }

        image.color = originalColor;
        content.alpha = 1;
        isHintLerping = false;
    }

    private IEnumerator DismissHint()
    {
        AudioSource.PlayClipAtPoint(pageAudio, transform.position);
        isHintLerping = true;
        doDismissHint = false;
        float initialHeight = -rectTransform.rect.height;
        float progress = 1;
        while (progress > 0)
        {
            rectTransform.anchoredPosition = new Vector2(
                0, Mathf.Lerp(initialHeight, currentMarginWidth, dialogCurve.Evaluate(progress))
            );
            progress -= Time.deltaTime / lerpDuration;
            yield return null;
        }

        rectTransform.anchoredPosition = new Vector2(0, initialHeight);
        isHintVisible = false;
        isHintLerping = false;
    }
}