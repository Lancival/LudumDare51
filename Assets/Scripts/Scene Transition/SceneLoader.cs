using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// A component which loads scenes and triggers scene loading transitions.
/// </summary>
/// <remarks>
/// Timeline of SceneLoader
///     LoadScene() is called
///     OnSceneTransition is invoked
///     Time passes (customizable)
///     OnSceneLoad is invoked
///     Time passes while the scene loads (unpredictable)
///     Next scene is activated
/// Loading a scene
///     Call the LoadScene() method to load a scene and automatically trigger the scene loading transition.
///     Parameters which are not provided default to the values stored on this component instead, such that multiple scripts can call LoadScene() and use the same defaults.
///     <see cref="LoadScene"> for more details about the method's parameters and their default values.
///     Example Usage:
///         SceneLoader loader = GetComponent<SceneLoader>();
///         loader.LoadScene("Name of Scene", 0.5f);
///     Note that each SceneLoader can only load a scene once - further LoadScene() calls have no effect.
/// Playing a transition at the end of the scene
///     Subscribe a function (via code or inspector) to the <see cref="OnSceneTransition"> UnityEvent to call that function before a scene load occurs.
///     The function should have a float parameter, representing the duration of the transition period, although this value may be ignored if being interrupted by a scene load is not problematic.
///     Example Usage:
///         loader.OnSceneTransition.AddListener(FunctionWhichTakesFloat);
///     Unsubscribing is optional since the SceneLoader will be destroyed when the next scene is activated.
/// Reacting to the scene load
///     Subscribe a parameterless function (via code or inspector) to the <see cref="OnSceneLoad"> UnityEvent to call that function just before scene loading begins.
///     Example Usage:
///          loader.OnSceneLoad.AddListener(FunctionWithNoParameters);
/// </remarks>
public class SceneLoader : MonoBehaviour
{
    private static SceneLoader _instance;
    public static SceneLoader instance => _instance;


    [Tooltip("Name of the default scene to be loaded, if no specific scene is specified.")]
    [SerializeField] private string defaultScene;

    [Tooltip("Duration of transition in seconds, before the scene load actually begins.")]
    [SerializeField] private float duration = 0.5f;

    [Tooltip("Event which is invoked when the transition begins. Any effects that occur just before scene loading should subscribe here.")]
    public UnityEvent<float> OnSceneTransition = new UnityEvent<float>();

    [Tooltip("Event which is invoked when scene loading begins. Any effects that occur during scene loading, or should stop at the end of the transition should subscribe here.")]
    public UnityEvent OnSceneLoad = new UnityEvent();

    // Whether this SceneLoader is currently loading a new scene
    private bool loading = false;

    void Awake() => _instance = this;

    /// <summary>
    /// A wrapper function for <see cref="LoadSceneAsync">, with multiple overloads.
    /// </summary>
    /// <param name="sceneName">The name of the scene to be loaded. Defaults to <see cref="defaultScene">.</param>
    /// <param name="transitionDuration">The number of seconds between the LoadScene() call and the scene load actually starting. Defaults to <see cref="duration">.</param>
    public void LoadScene(string sceneName, float transitionDuration)
    {
        if (!loading)
        {
            loading = true;
            StartCoroutine(LoadSceneAsync(sceneName, transitionDuration));
        }
    }

    // Wrapper method overloads
    public void LoadScene() => LoadScene(defaultScene, duration);
    public void LoadScene(string sceneName) => LoadScene(sceneName, duration);
    public void LoadScene(float transitionDuration) => LoadScene(defaultScene, transitionDuration);
    public void ReloadScene() => LoadScene(SceneManager.GetActiveScene().name, duration);
    public void ReloadScene(float transitionDuration) => LoadScene(SceneManager.GetActiveScene().name, transitionDuration);

    // Scene loading implementation
    private IEnumerator LoadSceneAsync(string sceneName, float transitionDuration)
    {
        // Error-checking
        if (sceneName == null)
        {
            Debug.LogError("The name of the scene to load was not provided.");
            yield break;
        }
        else if (transitionDuration < 0)
        {
            Debug.LogWarning("Duration of scene transition should be non-negative.");
            transitionDuration = 0;
        }

        // Scene transition
        OnSceneTransition?.Invoke(transitionDuration);
        yield return new WaitForSeconds(transitionDuration);

        // Attempt scene loading
        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(sceneName);
        if (sceneLoad == null)
        {
            Debug.LogError(string.Format("Scene loading operation failed, most likely because the scene \"{0}\" or the currently active scene was not included in the build settings.", sceneName));
            yield break;
        }

        // Wait for scene loading
        sceneLoad.allowSceneActivation = false;
        OnSceneLoad?.Invoke();
        while (sceneLoad.progress < 0.9f)
        {
            yield return null;
        }

        // Scene activation
        sceneLoad.allowSceneActivation = true;
    }
}
