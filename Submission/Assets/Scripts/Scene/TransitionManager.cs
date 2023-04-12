using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    [SerializeField] private Image mouseImage;
    [SerializeField] private Image keyboardImage;

    private void Start()
    {
        mouseImage.sprite = SceneState.mouseTransitionImage;
        keyboardImage.sprite = SceneState.keyboardTransitionImage;
        StartCoroutine(ScheduleLoadScene());
    }

    private IEnumerator ScheduleLoadScene()
    {
        yield return new WaitForSeconds(SceneState.holdDuration);
        SceneManager.LoadScene(SceneState.targetScene);
    }
}