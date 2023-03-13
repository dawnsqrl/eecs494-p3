using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class DialogDisplay : MonoBehaviour
{
    [SerializeField] private float initialTilt = 10;
    [SerializeField] private float lerpDuration = 0.5f;
    [SerializeField] private float lerpInterval = 0.2f;

    private RectTransform rectTransform;
    private float initialHeight;
    private Queue<DisplayDialogEvent> dialogQueue;
    private AnimationCurve dialogCurve;
    private DialogTitle dialogTitle;
    private DialogContent dialogContent;
    private DialogButtonContainer dialogButtonContainer;
    private bool isDialogLerping;
    private bool isDialogVisible;
    private bool doDismissDialog;

    private void Awake()
    {
        EventBus.Subscribe<DisplayDialogEvent>(e => dialogQueue.Enqueue(e));
        EventBus.Subscribe<DismissDialogEvent>(_ =>
        {
            if (!isDialogLerping)
            {
                doDismissDialog = true;
            }
        });
        rectTransform = GetComponent<RectTransform>();
        dialogQueue = new Queue<DisplayDialogEvent>();
        dialogCurve = Resources.Load<AnimationCurveAsset>("Curves/DialogCurve");
        dialogTitle = GetComponentInChildren<DialogTitle>();
        dialogContent = GetComponentInChildren<DialogContent>();
        dialogButtonContainer = GetComponentInChildren<DialogButtonContainer>();
    }

    private void Start()
    {
        SetInitialHeight();
        rectTransform.rotation = Quaternion.Euler(0, 0, initialTilt);
        isDialogLerping = false;
        isDialogVisible = false;
        doDismissDialog = false;
    }

    private void Update()
    {
        if (!isDialogLerping)
        {
            if (!isDialogVisible)
            {
                SetInitialHeight();
                if (dialogQueue.Count > 0)
                {
                    StartCoroutine(DisplayDialog(dialogQueue.Dequeue()));
                }
            }
            else if (isDialogVisible && doDismissDialog)
            {
                StartCoroutine(DismissDialog());
            }
            else if (doDismissDialog)
            {
                doDismissDialog = false;
            }
        }
    }

    private void SetInitialHeight()
    {
        Rect rect = rectTransform.rect;
        initialHeight = (
            Screen.height + new Vector2(rect.width, rect.height).magnitude
            * Mathf.Cos(Mathf.Atan(rect.width / rect.height) - initialTilt * Mathf.Deg2Rad)
        ) / 2;
        rectTransform.anchoredPosition = new Vector2(0, initialHeight);
    }

    private IEnumerator DisplayDialog(DisplayDialogEvent e)
    {
        isDialogLerping = true;
        dialogTitle.SetTitle(e.title);
        dialogContent.SetContent(e.content);
        dialogButtonContainer.SetButton(e.buttons);
        float progress = 0;
        while (progress < 1)
        {
            float curveValue = dialogCurve.Evaluate(progress);
            rectTransform.anchoredPosition = new Vector2(0, Mathf.Lerp(initialHeight, 0, curveValue));
            rectTransform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(initialTilt, 0, curveValue));
            progress += Time.deltaTime / lerpDuration;
            yield return null;
        }

        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.rotation = Quaternion.Euler(Vector3.zero);
        isDialogVisible = true;
        isDialogLerping = false;
    }

    private IEnumerator DismissDialog()
    {
        isDialogLerping = true;
        doDismissDialog = false;
        float progress = 1;
        while (progress > 0)
        {
            float curveValue = dialogCurve.Evaluate(progress);
            rectTransform.anchoredPosition = new Vector2(0, Mathf.Lerp(initialHeight, 0, curveValue));
            rectTransform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(initialTilt, 0, curveValue));
            progress -= Time.deltaTime / lerpDuration;
            yield return null;
        }

        rectTransform.anchoredPosition = new Vector2(0, initialHeight);
        rectTransform.rotation = Quaternion.Euler(0, 0, initialTilt);
        if (dialogQueue.Count > 0)
        {
            yield return new WaitForSeconds(lerpInterval);
        }

        isDialogVisible = false;
        isDialogLerping = false;
    }
}