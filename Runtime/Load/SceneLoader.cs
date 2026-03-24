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

    [SerializeField][Min(0f)]  private float prePostWaitTime = 0.5f;
    [SerializeField][Min(0f)] private float minLoadTime = 1;
    [SerializeField][Range(0f, 0.9f)] private float _realLoadingOnBar = 0.8f;
    [SerializeField] private Slider _loadSlider;
    [SerializeField] private TMP_Text _sceneName;
    [SerializeField] private TMP_Text _loadPercentage;

    public static bool IsLoading { get; private set; } = false;

    public static RestoreFlag CurrentRestoreFlag { get; private set; }

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

        if (MenuData.HasInstance)
            MenuData.Instance.IncreasePause();
        
        float prevTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        // test SceneToLoad ??= "LoadScene";

        // wait prePostWaitTime before trying to load
        yield return new WaitForSecondsRealtime(prePostWaitTime);

        AsyncOperation op;

        // unload all scenes except load scene
        for (int i = SceneManager.sceneCount - 1; i >= 0; i--)
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
        float startTime = Time.unscaledTime;
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
                _loadSlider.value = Mathf.Lerp(_loadSlider.minValue, _loadSlider.maxValue * _realLoadingOnBar, progress);

            if (_loadPercentage != null)
            {
                int percent = Mathf.RoundToInt(progress * _realLoadingOnBar * 100f);
                _loadPercentage.text = percent.ToString("00");
            }

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
        float leftTime = minLoadTime - (Time.unscaledTime - startTime);
        leftTime = Mathf.Max(0, leftTime);

        float timer = 0;

        while (timer < leftTime)
        {
            timer += Time.unscaledDeltaTime;
            float finalProgress = Mathf.Clamp01(timer / leftTime);

            if (_loadSlider != null)
                _loadSlider.value = Mathf.Lerp(_loadSlider.maxValue * _realLoadingOnBar, _loadSlider.maxValue, finalProgress);
            // Debug.Log("timer? " + timer);

            if (_loadPercentage != null)
            {
                int percent = Mathf.RoundToInt(
                    Mathf.Lerp(_realLoadingOnBar * 100f, 100f, finalProgress)
                );
                _loadPercentage.text = percent.ToString("00");
            }

            yield return null;
        }

        // SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneToLoad));

        //unload loading scene
        // onFinishLoad.Invoke();
        yield return new WaitForSecondsRealtime(prePostWaitTime);

        if (_loadPercentage != null)
            _loadPercentage.text = "100";

        // why does unload scene async's async object not correctly return completed when using it in a yield return or loop?
        SceneManager.UnloadSceneAsync(_loadScene);

        IsLoading = false;
        CurrentRestoreFlag = null;
        Time.timeScale = prevTimeScale;

        if (MenuData.HasInstance)
            MenuData.Instance.IncreasePause();

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