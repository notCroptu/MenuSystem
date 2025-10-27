using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : Menu
{
    [Header("Pause")]
    [InfoBox("The pause game object must not be the same as the pause script's. ")]
    [SerializeField] private GameObject _pause;

    private void Start()
    {
        Continue();
    }

    /// <summary>
    /// Runs on late update incase any inputs with the same keys are ran at the same time.
    /// </summary>
    private void LateUpdate()
    {
        if (  SceneManager.GetActiveScene().name != _mainMenu && InputManager.Pause() )
        {
            if ( _pause.activeSelf )
                Continue();
            else
            {
                _pause.SetActive( true );
                Time.timeScale = 0f;
                // InputManager.Paused = true;
            }
        }
        // Debug.Log("time scale?" + Time.timeScale);
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

    public void OnDestroy()
    {
        Debug.Log("Destroying pause");
        Continue();        
    }
}
