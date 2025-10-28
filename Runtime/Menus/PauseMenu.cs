using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Pause menu needs to be contained in a DDOL object (don't destroy on load (new scene) ) with Settings Menu.
/// </summary>
public class PauseMenu : Menu
{
    [Header("Pause")]
    [InfoBox("The pause game object must not be the same as the pause script's. Should be one game object above in the hierarchy. ")]
    [SerializeField] private GameObject _pause;
    [SerializeField][Range(0f, 1f)] private float _timeScaleMultiplier = 0f;

    // It's to check if the game is paused to stop other behaviors, mostly stopping the player from being able to use inputs and enumerators (they continue running despite of time scale changes.)
    public bool Paused => Count != 0;

    // Some interactions require pausing despite no pause stack, later add a paused bool check
    public int _pauseCount = 0;
    public int Count
    {
        get { return _pauseCount; }
        set
        {
            Debug.Log("Pause count changing to: " + value);
            _pauseCount = value;
        }
    }

    private void Start()
    {
        Continue();
    }

    /// <summary>
    /// Runs on late update in case any inputs with the same keys are ran at the same time.
    /// </summary>
    private void LateUpdate()
    {
        if (SceneManager.GetActiveScene().name != _mainMenu && Input.GetKeyDown(KeyCode.Escape)) // need to change hard coded escape key to a chose yourself at the top.
        {
            if (_pause.activeSelf)
                Continue();
            else
            {
                _pause.SetActive(true);
                Time.timeScale *= _timeScaleMultiplier; // we only stop timescale on pause menu because for interactions like reading a book for example, in game, still want the game to be running around them. 
                Count++;
            }
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
        base.Quit(); // on subscribes all DDOLs (except the object's own)
        SceneLoader.Load(_mainMenu);
    }

    /// <summary>
    /// This button will resume the game from where the player left off.
    /// </summary>
    public override void Continue()
    {
        Time.timeScale = 1f;
        // InputManager.Paused = false;

        _settings.Continue();
        _pause.SetActive(false);

        Count--;

        // Debug.Log("unloading");
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
        Continue();
    }
    
    private void OnApplicationQuit()
    {
        _pauseCount = 0;
    }
}
