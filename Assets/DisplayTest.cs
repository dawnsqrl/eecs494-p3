using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisplayTest : MonoBehaviour
{
    // This variable will store the target scene index you want to load
    private int targetSceneIndex;

    private void Awake()
    {
        // Set this GameObject to not be destroyed when a new scene is loaded
        DontDestroyOnLoad(gameObject);

        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoadSceneOnAllDisplays(int sceneIndex)
    {
        // Store the target scene index
        targetSceneIndex = sceneIndex;

        // Start the process of loading the new scene on the main display
        SceneManager.LoadScene(sceneIndex);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If the scene that was just loaded is the target scene
        if (scene.buildIndex == targetSceneIndex)
        {
            // Loop through all displays and load the new scene on each one
            for (int i = 1; i < Display.displays.Length; i++)
            {
                StartCoroutine(LoadSceneOnDisplay(i, targetSceneIndex));
            }
        }
    }

    private IEnumerator LoadSceneOnDisplay(int displayIndex, int sceneIndex)
    {
        // Create a new scene for the display
        Scene displayScene = SceneManager.CreateScene("DisplayScene" + displayIndex);

        // Set the new scene as the active scene
        SceneManager.SetActiveScene(displayScene);

        // Load the content of the target scene into the new scene
        yield return SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);

        // Set the new scene to be shown on the display
        Display.displays[displayIndex].Activate();
    }
}
