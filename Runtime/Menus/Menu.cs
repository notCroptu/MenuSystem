using NaughtyAttributes;
using UnityEngine;

public abstract class Menu : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] protected Canvas _menuCanvas;
    [Scene][SerializeField] protected string _mainMenuScene;

    protected SettingsMenu _settings;

    private void Awake()
    {
        if (_menuCanvas == null)
        {
            _menuCanvas = GetComponent<Canvas>();
            if (_menuCanvas == null)
                Debug.LogWarning(name + " missing _menuCanvas reference.");
        }

        UpdateCamera();

        _settings = FindFirstObjectByType<SettingsMenu>();

        if (_settings == null)
            Debug.LogWarning(name + " could not find SettingsMenu in scene.");
    }

    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void UpdateCamera()
    {
        if (_menuCanvas == null) return;

        if (ActiveUICam.ActiveUICamera != null) // if an active UI cam exists, then set it as the focus, with screen space as camera so the ui receives post processing
        {
            _menuCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            _menuCanvas.worldCamera = ActiveUICam.ActiveUICamera;
            _menuCanvas.planeDistance = 1f;
        }
        else
        {
            _menuCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _menuCanvas.worldCamera = null;
        }
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        Invoke(nameof(UpdateCamera), 0.1f);
    }

    private void Update()
    {
        if (_menuCanvas != null && _menuCanvas.worldCamera == null)
            UpdateCamera();
    }


    /// <summary>
    /// Opens the Settings menu, these changes will be applied everywhere in all scenes of the game.
    /// </summary>
    public void OpenSettings()
    {
        if (_settings == null)
        {
            _settings = FindFirstObjectByType<SettingsMenu>();
            if (_settings == null)
            {
                Debug.LogWarning(name + " could not open SettingsMenu no instance found.");
                return;
            }
        }

        _settings.TurnOnSettings();
    }

    public abstract void Continue();
    public virtual void Quit()
    {
        // It's important to delete all DDOLs except our own
        // Deleting game time DDOLs removes "progress" that wasn't saved from interfering when loading or starting a new game
        // Not deleting our own, ensures pause will not be deleted, keeping settings data during play

        DontDestroyOnLoad[] ddols = FindObjectsByType<DontDestroyOnLoad>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (DontDestroyOnLoad ddol in ddols)
        {
            if (ddol != null && ddol.gameObject != gameObject)
                Destroy(ddol.gameObject);
        }
    }
}
