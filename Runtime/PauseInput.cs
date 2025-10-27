using UnityEngine;

/// <summary>
/// When adding this component to a UI that turns on and off, it keeps the input manager paused in a stack.
/// </summary>
public class PauseInput : MonoBehaviour
{
    private void OnEnable()
    {
        InputManager.PauseCount++;
        Debug.Log("Adding pause count: " + InputManager.PauseCount + " at GO: " + gameObject);
    }

    private void OnDisable()
    {
        InputManager.PauseCount--;
        Debug.Log("disable removing pause count: " + InputManager.PauseCount + " at GO: " + gameObject);
    }

    private void OnDestroy()
    {
        if (gameObject != null && gameObject.activeSelf)
        {
            // InputManager.PauseCount--;
            Debug.Log("destroy removing pause count: " + InputManager.PauseCount + " at GO: " + gameObject);
        }
    }
}
