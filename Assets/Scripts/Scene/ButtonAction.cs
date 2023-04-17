using UnityEngine;

public class ButtonAction : MonoBehaviour
{
    private Sprite mouseTransitionImage;
    private Sprite mouseGameImage;
    private Sprite keyboardTransitionImage;
    private Sprite keyboardGameImage;

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
                1, 0, StringPool.mainGameScene, mouseGameImage, keyboardGameImage
            );
            EventBus.Publish(new TransitSceneEvent());
        }
    }

    public void OnClickTutorial()
    {
        SceneState.SetTransition(
            1, 0, StringPool.mainGetReadyScene, mouseTransitionImage, keyboardTransitionImage
        );
        EventBus.Publish(new TransitSceneEvent());
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }

    public void OnClickReturn()
    {
        SceneState.SetTransition(
            1, 2, StringPool.mainMenuScene, mouseGameImage, keyboardGameImage
        );
        EventBus.Publish(new TransitSceneEvent());
    }

    public void OnClickRestart()
    {
        SceneState.SetTransition(
            1, 0, StringPool.mainGameScene, mouseGameImage, keyboardGameImage
        );
        EventBus.Publish(new TransitSceneEvent());
    }
}