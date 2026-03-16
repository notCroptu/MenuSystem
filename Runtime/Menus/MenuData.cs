using NaughtyAttributes;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(DontDestroyOnLoad))]
public class MenuData : MonoBehaviour
{
    private static MenuData _instance;
    public static MenuData Instance
    {
        get {
            if (_instance == null)
                Debug.LogWarning("MenuData instance was requested but none exists.");

            return _instance;
        }
        private set => _instance = value;
    }

    public static bool HasInstance
    {
        get
        {
            if (Instance == null)
                Debug.LogWarning("MenuData instance was inquired but none exists.");

            return Instance != null;
        }
    }

    [Scene][SerializeField] private string gameStartScene;
    public string GameStartScene => gameStartScene;

    [Scene][SerializeField] private string mainMenuScene;
    public string MainMenuScene => mainMenuScene;

    [field: SerializeField] public SettingsMenu SettingsMenu { get; private set; }
    [field: SerializeField] public PauseMenu PauseMenu { get; private set; }

    [field: SerializeField] public bool OverrideMode { get; private set; } = false;

    [Header("Colors")]
    
    [field: SerializeField] public Color LinkHoverColor { get; private set; } = Color.cyan;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void IncreasePause()
    {
        if (PauseMenu == null)
        {
            PauseMenu = FindFirstObjectByType<PauseMenu>();
            if (PauseMenu == null)
            {
                Debug.LogWarning(name + " could not open PauseMenu no instance found.");
                return;
            }
        }

        PauseMenu.Count++;
    }

    public void DecreasePause()
    {
        if (PauseMenu == null)
        {
            PauseMenu = FindFirstObjectByType<PauseMenu>();
            if (PauseMenu == null)
            {
                Debug.LogWarning(name + " could not open PauseMenu no instance found.");
                return;
            }
        }

        PauseMenu.Count--;
    }
    
    public void OpenSettings()
    {
        if (SettingsMenu == null)
        {
            SettingsMenu = FindFirstObjectByType<SettingsMenu>();
            if (SettingsMenu == null)
            {
                Debug.LogWarning(name + " could not open SettingsMenu no instance found.");
                return;
            }
        }

        SettingsMenu.TurnOnSettings();
    }
}
