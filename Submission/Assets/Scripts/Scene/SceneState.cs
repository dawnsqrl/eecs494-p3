using UnityEngine;

public static class SceneState
{
    public static bool isTutorialAccessed = false;

    public static float transitionDuration = 1;
    public static float holdDuration = 5;
    public static string targetScene = "";
    public static Sprite mouseTransitionImage;
    public static Sprite keyboardTransitionImage;

    public static void SetTransition(float _transitionDuration, float _holdDuration,
        string _targetScene, Sprite _mouseTransitionImage, Sprite _keyboardTransitionImage)
    {
        SceneState.transitionDuration = _transitionDuration;
        SceneState.holdDuration = _holdDuration;
        SceneState.targetScene = _targetScene;
        SceneState.mouseTransitionImage = _mouseTransitionImage;
        SceneState.keyboardTransitionImage = _keyboardTransitionImage;
    }
}