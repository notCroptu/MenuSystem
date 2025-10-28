using UnityEngine;

/// <summary>
/// When adding this component to a UI that turns on and off, it keeps the input manager paused in a stack.
/// </summary>
public class PauseInput : MonoBehaviour
{
    private Pause _pause;

    private void FindPause()
    {
        if (_pause == null)
            _pause = FindFirstObjectByType<Pause>();
    }

    private void OnEnable()
    {
        FindPause();

        _pause.Count++;
        Debug.Log("Adding pause count: " + _pause.Count + " at GO: " + gameObject);
    }

    private void OnDisable()
    {
        FindPause();

        _pause.Count--;
        Debug.Log("disable removing pause count: " + _pause.Count + " at GO: " + gameObject);
    }

    private void OnDestroy()
    {
        FindPause();

        if (gameObject != null && gameObject.activeSelf)
        {
            Debug.Log("destroy removing pause count: " + _pause.Count + " at GO: " + gameObject);
        }
    }
}
