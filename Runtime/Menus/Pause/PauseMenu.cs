using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : Menu
{
    [InfoBox("The pause game object must not be the same as the pause script's. Should be one game object above in the hierarchy. \n Pause menu needs to be contained in a DDOL object (don't destroy on load (new scene) ).")]

    [Header("Pause")]
    [SerializeField][Range(0f, 1f)] private float _timeScaleMultiplier = 0f;
    [SerializeField] private KeyCode _pauseToggleKey = KeyCode.Escape;
    private float _previousTimeScale;

    // It's to check if the game is paused to stop other behaviors, mostly stopping the player from being able to use inputs and enumerators (they continue running despite of time scale changes.)
    public bool Paused => Count != 0;
    public int _pauseCanvasCount = 0;
    public int Count
    {
        get { return _pauseCanvasCount; }
        set
        {
            Debug.Log("Pause count changing to: " + value);
            _pauseCanvasCount = value;
        }
    }

    private void Start()
    {
        if (_menuCanvas == null)
            Debug.LogWarning(name + " missing _pauseCanvas GameObject.");
        else
            _menuCanvas.gameObject.SetActive(false);
    }

    /// <summary>
    /// Runs on late update in case any inputs with the same keys are ran at the same time.
    /// </summary>
    private void LateUpdate()
    {
        if (_menuCanvas == null)
            return;

        if (SceneManager.GetActiveScene().name != _mainMenuScene && Input.GetKeyDown(_pauseToggleKey)) // the fact it uses get key down is not very mobile friendly, i might wanna add functionality later
        {
            if (_menuCanvas.gameObject.activeSelf) // checking is pause object is active acts like a toggle.
                Continue();
            else
                OpenPause();
        }
    }

    /// <summary>
    /// This button will save the current game’s data and go back to the main menu.
    /// </summary>
    public override void Quit()
    {
        // preform save actions

        Save();

        Continue();

        Debug.Log("loading main");
        base.Quit(); // un-subscribes all DDOLs (except the object's own)
        SceneLoader.Load(_mainMenuScene);
    }

    public void OpenPause()
    {
        if (_menuCanvas == null) return;

        _menuCanvas.gameObject.SetActive(true);

        if (_timeScaleMultiplier == 0f)
        {
            _previousTimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale *= _timeScaleMultiplier; // we only assume we stop timescale in pause menu because for other UIs like reading a book for example where pause input might increment, in game, you still want the game to be running around them. 
        }

        Count++;
    }

    /// <summary>
    /// This button will resume the game from where the player left off.
    /// </summary>
    public override void Continue()
    {
        if (_settings != null)
            _settings.Continue();
        else
            Debug.LogWarning(name + " SettingsMenu reference missing in PauseMenu.");
        
        if (_menuCanvas != null)
            _menuCanvas.gameObject.SetActive(false);

        if (_timeScaleMultiplier == 0f)
        {
            Time.timeScale = _previousTimeScale;
        }
        else
        {
            Time.timeScale /= _timeScaleMultiplier;
        }

        Count = Mathf.Max(0, Count - 1);
    }

    /// <summary>
    /// Saves the current progress.
    /// </summary>
    public void Save()
    {

    }

    /// <summary>
    /// Loads the last saved progress.
    /// </summary>
    public void Load()
    {

    }

    private void OnDestroy()
    {
        Debug.Log("Destroying pause");
        Count = 0;
    }
    
    private void OnApplicationQuit()
    {
        Count = 0;
    }
}
