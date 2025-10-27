using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ActiveUICam : MonoBehaviour
{
    public static Camera ActiveUICamera { get; private set; }
    private Camera _uiCamera;
    private void Awake()
    {
        _uiCamera = GetComponent<Camera>();
    }

    private void OnEnable()
    {
        if (ActiveUICamera == null)
            ActiveUICamera = _uiCamera;
    }

    private void OnDisable()
    {
        if (ActiveUICamera == _uiCamera)
            ActiveUICamera = null;
    }

    private void OnDestroy()
    {
        if ( ActiveUICamera == _uiCamera )
            ActiveUICamera = null;
    }

    private void OnApplicationQuit()
    {
        if ( ActiveUICamera != null )
            ActiveUICamera = null;
    }
}