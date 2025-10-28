using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps persistent game objects like managers between scenes. They are all destroyed by pause menu when loading main.
/// </summary>
public class DontDestroyOnLoad : MonoBehaviour
{
    private static HashSet<string> _instances;
    private void Awake()
    {
        _instances ??= new();

        if (!_instances.Contains(gameObject.name))
        {
            Debug.Log("Adding new DDOL: " + gameObject.name);
            _instances.Add(gameObject.name);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning(name + " duplicate DDOL detected, destroying this instance: " + gameObject.name);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (_instances.Contains(gameObject.name))
        {
            Debug.Log("Removing DDOL: " + gameObject.name);
            _instances.Remove(gameObject.name);
        }
    }

    private void OnApplicationQuit()
    {
        if (_instances != null)
            _instances.Clear();
    }
}
