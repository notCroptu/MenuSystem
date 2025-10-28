using NaughtyAttributes;
using UnityEngine;

public class MainMenu : Menu
{
    [Header("Main")]
    [Scene][SerializeField] private string _gameScene;

    /// <summary>
    /// Loads the previous game files and starts the game off from where the player left off.
    /// </summary>
    public override void Continue()
    {
        // load previous save
    }

    /// <summary>
    /// Saves any progress and closes the game window.
    /// </summary>
    public override void Quit()
    {
        base.Quit();

        Application.Quit();
    }

    /// <summary>
    /// Clears the game files and loads a fresh game.
    /// </summary>
    public void NewGame()
    {
        if (string.IsNullOrEmpty(_gameScene))
        {
            Debug.LogWarning(name + " _game scene not assigned in MainMenu.");
            return;
        }


        SceneLoader.Load(_gameScene);
    }
}
