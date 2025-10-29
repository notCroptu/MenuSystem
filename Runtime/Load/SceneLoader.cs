using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

/// <summary>
/// Scene loader must be in a dedicated scene where the load progress will be shown.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    private const string _loadScene = "LoadScene";
    public static string SceneToLoad { get; private set; }

    [SerializeField] private float prePostWaitTime = 0.5f;
    [SerializeField] private float minLoadTime = 1;
    [SerializeField] private Slider _loadSlider;
    [SerializeField] private TMP_Text _sceneName;

    public static bool IsLoading { get; private set; } = false;

    public static RestoreFlag CurrentRestoreFlag { get; private set; }

    private PauseMenu _pause;

    public static void Load(string scene, RestoreFlag restoreFlag = null)
    {
        if (!CanLoadScene(scene))
        {
            Debug.LogWarning("Scene " + scene + " is not present in the build settings. Aborting. ");
            return;
        }

        if (!CanLoadScene(_loadScene))
        {
            Debug.LogWarning("Load scene is not present in the build settings. Aborting and loading through Scene manager. ");
            SceneManager.LoadScene(scene);
            return;
        }

        Debug.Log("DESTROYED: Loading new");
        if (IsLoading) return;
        IsLoading = true;

        CurrentRestoreFlag = restoreFlag;
        if (CurrentRestoreFlag != null)
            restoreFlag.IsRestored = false;

        SceneToLoad = scene;
        SceneManager.LoadSceneAsync(_loadScene, LoadSceneMode.Additive);
    }

    /// <summary>
    /// When starting up the load scene, then it starts unloading previous scenes and loads the new scene
    /// </summary>
    private IEnumerator Start()
    {
        if (_sceneName != null)
            _sceneName.text = "Loading " + SceneToLoad + "...";

        if (_pause == null)
            _pause = FindFirstObjectByType<PauseMenu>();

        _pause.Count++;
        // test SceneToLoad ??= "LoadScene";

        // wait prePostWaitTime before trying to load
        yield return new WaitForSeconds(prePostWaitTime);

        AsyncOperation op;

        // unload all scenes except load scene
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name != _loadScene)
            {
                Debug.Log("unloading async: " + scene.name);
                op = SceneManager.UnloadSceneAsync(scene.name);
                yield return new WaitWhile(() => !op.isDone);
            }
        }

        yield return Resources.UnloadUnusedAssets();
        System.GC.Collect();

        //  start counting load time
        float startTime = Time.time;
        // start loading target scene without enabling it
        op = SceneManager.LoadSceneAsync(SceneToLoad, LoadSceneMode.Additive);
        op.allowSceneActivation = false;

        float progress;

        // progress visualization
        Debug.Log("one ");
        while (!op.isDone)
        {
            // this line remaps 0-0.9 (AsyncOperation.progress returns a value in this range) into a value between 0-1
            progress = Mathf.InverseLerp(0, 0.9f, op.progress);
            if (_loadSlider != null)
                _loadSlider.value = Mathf.Lerp(_loadSlider.minValue, _loadSlider.maxValue * 0.8f, progress);

            if (progress >= 1)
            {
                Debug.Log("progress activated");
                op.allowSceneActivation = true;
            }
            yield return null;
        }

        // SceneManager.SetActiveScene(SceneManager.GetSceneByName(_loadScene));

        Debug.Log("two ");
        if (CurrentRestoreFlag != null)
            yield return new WaitUntil(() => CurrentRestoreFlag.IsRestored);

        // give it a min loading time, as loading immediately apparently gives a "pop" effect
        float leftTime = minLoadTime - (Time.time - startTime);
        leftTime = Mathf.Max(0, leftTime);

        float timer = 0;

        while (timer < leftTime)
        {
            timer += Time.deltaTime;
            float finalProgress = Mathf.Clamp01(timer / leftTime);

            if (_loadSlider != null)
                _loadSlider.value = Mathf.Lerp(_loadSlider.maxValue * 0.8f, _loadSlider.maxValue, finalProgress);
            // Debug.Log("timer? " + timer);
            yield return null;
        }

        // SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneToLoad));

        //unload loading scene
        // onFinishLoad.Invoke();
        yield return new WaitForSeconds(prePostWaitTime);

        // why does unload scene async's async object not correctly return completed when using it in a yield return or loop?
        SceneManager.UnloadSceneAsync(_loadScene);

        IsLoading = false;
        CurrentRestoreFlag = null;
        _pause.Count--;

        if (_sceneName != null)
            _sceneName.text = "";
    }
    
    public static bool CanLoadScene(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = Path.GetFileNameWithoutExtension(path);

            if (name.Equals(sceneName, System.StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }
}