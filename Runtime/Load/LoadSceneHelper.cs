using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// Use this to load scenes from components with actions, like buttons. Add and action and connect this component with the method Load Scene.
/// </summary>
public class LoadSceneHelper : MonoBehaviour
{
    [Scene][SerializeField] private string _scene;
    public void LoadScene()
    {
        // SceneManager.LoadScene(_loadScene);
        SceneLoader.Load(_scene);
    }
}