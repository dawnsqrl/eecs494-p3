using UnityEngine;

public class TutorialStateManager : MonoBehaviour
{
    [SerializeField] private GameObject tutorialButton;

    private void Start()
    {
        tutorialButton.SetActive(SceneState.isTutorialAccessed);
    }
}