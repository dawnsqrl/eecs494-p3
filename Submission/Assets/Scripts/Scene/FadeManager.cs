using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    [SerializeField] private GameObject mouseOverlay;
    [SerializeField] private GameObject keyboardOverlay;

    private AnimationCurve dialogCurve;
    private Canvas mouseCanvas;
    private Image mouseImage;
    private Canvas keyboardCanvas;
    private Image keyboardImage;

    private void Awake()
    {
        EventBus.Subscribe<TransitSceneEvent>(_OnTransitScene);
        dialogCurve = Resources.Load<AnimationCurveAsset>("Curves/DialogCurve");
        mouseCanvas = mouseOverlay.GetComponentInChildren<Canvas>();
        mouseImage = mouseOverlay.GetComponentInChildren<Image>();
        keyboardCanvas = keyboardOverlay.GetComponentInChildren<Canvas>();
        keyboardImage = keyboardOverlay.GetComponentInChildren<Image>();
        mouseCanvas.targetDisplay = 0;
        if (Display.displays.Length > 1)
        {
            Display.displays[1].Activate();
            keyboardCanvas.targetDisplay = 1;
        }
        else
        {
            keyboardCanvas.enabled = false;
        }
    }

    private void Start()
    {
        if (mouseCanvas.enabled && SceneState.mouseTransitionImage != null)
        {
            mouseImage.sprite = SceneState.mouseTransitionImage;
            StartCoroutine(FadeIn(mouseCanvas, mouseImage));
        }
        else
        {
            mouseCanvas.enabled = false;
        }

        if (keyboardCanvas.enabled && SceneState.keyboardTransitionImage != null)
        {
            keyboardImage.sprite = SceneState.keyboardTransitionImage;
            StartCoroutine(FadeIn(keyboardCanvas, keyboardImage));
        }
        else
        {
            keyboardCanvas.enabled = false;
        }
    }

    private void _OnTransitScene(TransitSceneEvent e)
    {
        if (SceneState.mouseTransitionImage is not null)
        {
            mouseCanvas.enabled = true;
            mouseImage.sprite = SceneState.mouseTransitionImage;
            StartCoroutine(FadeOut(mouseImage));
        }

        if (Display.displays.Length > 1 && SceneState.keyboardTransitionImage is not null)
        {
            keyboardCanvas.enabled = true;
            keyboardImage.sprite = SceneState.keyboardTransitionImage;
            StartCoroutine(FadeOut(keyboardImage));
        }
    }

    private IEnumerator FadeIn(Canvas canvas, Image image)
    {
        float progress = 0;
        Color startColor = new Color(1, 1, 1, 1);
        Color endColor = new Color(1, 1, 1, 0);
        while (progress < 1)
        {
            image.color = Color.Lerp(startColor, endColor, dialogCurve.Evaluate(progress));
            progress += Time.deltaTime / SceneState.transitionDuration;
            yield return null;
        }

        image.color = endColor;
        canvas.enabled = false;
    }

    private IEnumerator FadeOut(Image image)
    {
        float progress = 0;
        Color startColor = new Color(1, 1, 1, 0);
        Color endColor = new Color(1, 1, 1, 1);
        while (progress < 1)
        {
            image.color = Color.Lerp(startColor, endColor, dialogCurve.Evaluate(progress));
            progress += Time.deltaTime / SceneState.transitionDuration;
            yield return null;
        }

        image.color = endColor;
        SceneManager.LoadScene("MainTransition");
    }
}