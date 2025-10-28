using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : Menu
{
    [Header("Pause")]
    [InfoBox("The pause game object must not be the same as the pause script's. Should be one game object above in the hierarchy. \n Pause menu needs to be contained in a DDOL object (don't destroy on load (new scene) ).")]
    [SerializeField] private GameObject _pause;
    [SerializeField][Range(0f, 1f)] private float _timeScaleMultiplier = 0f;
    private float _previousTimeScale;

    // It's to check if the game is paused to stop other behaviors, mostly stopping the player from being able to use inputs and enumerators (they continue running despite of time scale changes.)
    public bool Paused => Count != 0;
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
        _pause.SetActive(false);
    }

    /// <summary>
    /// Runs on late update in case any inputs with the same keys are ran at the same time.
    /// </summary>
    private void LateUpdate()
    {
        if (SceneManager.GetActiveScene().name != _mainMenu && Input.GetKeyDown(KeyCode.Escape)) // TODO: need to change hard coded escape key to a chose yourself at the top.
        {
            if (_pause.activeSelf) // checking is pause object is active acts like a toggle.
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
        SceneLoader.Load(_mainMenu);
    }

    public void OpenPause()
    {
        _pause.SetActive(true);

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
        _settings.Continue();
        _pause.SetActive(false);

        if (_timeScaleMultiplier == 0f)
        {
            Time.timeScale = _previousTimeScale;
        }
        else
        {
            Time.timeScale /= _timeScaleMultiplier;
        }

        Count--;
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
    }
    
    private void OnApplicationQuit()
    {
        _pauseCount = 0;
    }
}
