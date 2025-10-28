using NaughtyAttributes;
using UnityEngine;

public abstract class Menu : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] private Canvas canvas;
    [Scene][SerializeField] protected string _mainMenu;

    protected SettingsMenu _settings;

    private void Awake()
    {
        if (canvas == null)
            canvas = GetComponent<Canvas>();
        UpdateCamera();

        _settings = FindFirstObjectByType<SettingsMenu>();
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
        if (ActiveUICam.ActiveUICamera != null) // if an active UI cam exists, then set it as the focus, with screen space as camera so the ui receives post processing
        {
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = ActiveUICam.ActiveUICamera;
            canvas.planeDistance = 1f;
        }
        else
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.worldCamera = null;
        }
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        Invoke(nameof(UpdateCamera), 0.1f);
    }

    private void Update()
    {
        if (canvas.worldCamera == null)
            UpdateCamera();
    }


    /// <summary>
    /// Opens the Settings menu, these changes will be applied everywhere in all scenes of the game.
    /// </summary>
    public void OpenSettings()
    {
        if (_settings == null)
            _settings = FindFirstObjectByType<SettingsMenu>();

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
            if (ddol != gameObject)
                Destroy(ddol.gameObject);
    }
}
