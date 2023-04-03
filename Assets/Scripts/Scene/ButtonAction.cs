using UnityEngine;

public class ButtonAction : MonoBehaviour
{
    private Sprite mouseTransitionImage;
    private Sprite mouseGameImage;
    private Sprite keyboardTransitionImage;
    private Sprite keyboardGameImage;
    private float transitionDuration = 1;
    private float holdDuration = 0;

    private void Awake()
    {
        mouseTransitionImage = Resources.Load<Sprite>("Sprites/Background/MouseTransition");
        mouseGameImage = Resources.Load<Sprite>("Sprites/Background/MouseGame");
        keyboardTransitionImage = Resources.Load<Sprite>("Sprites/Background/KeyboardTransition");
        keyboardGameImage = Resources.Load<Sprite>("Sprites/Background/KeyboardGame");
    }

    public void OnClickStart()
    {
        if (!SceneState.isTutorialAccessed)
        {
            SceneState.isTutorialAccessed = true;
            OnClickTutorial();
        }
        else
        {
            SceneState.SetTransition(
                transitionDuration, holdDuration, "MainGame",
                mouseGameImage, keyboardGameImage
            );
            EventBus.Publish(new TransitSceneEvent());
        }
    }

    public void OnClickTutorial()
    {
        SceneState.SetTransition(
            transitionDuration, holdDuration, "MainGetReady",
            mouseTransitionImage, keyboardTransitionImage
        );
        EventBus.Publish(new TransitSceneEvent());
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }
}