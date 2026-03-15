using UnityEngine;

public class MainMenu : Menu
{
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
    }

    /// <summary>
    /// Clears the game files and loads a fresh game.
    /// </summary>
    public void NewGame()
    {
        if (!MenuData.HasInstance) return;

        if (string.IsNullOrEmpty(MenuData.Instance.GameStartScene))
        {
            Debug.LogWarning(name + " _game scene not assigned in MenuData.");
            return;
        }


        SceneLoader.Load(MenuData.Instance.GameStartScene);
    }
}
