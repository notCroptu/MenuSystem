using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps persistent game objects like managers between scenes. They are all destroyed by pause menu when loading main.
/// </summary>
public class DontDestroyOnLoad : MonoBehaviour
{
    public static Dictionary<string, GameObject> Instances { get; private set; }
    private void Awake()
    {
        Instances ??= new();

        if (!Instances.ContainsKey(gameObject.name))
        {
            Debug.Log("Adding new DDOL: " + gameObject.name);
            Instances.Add(gameObject.name, gameObject);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // Remove entry only if the object's string matches the GO reference  in the dictionary.
        // This prevents a destroying duplicate from unregistering the real instance.
        if (Instances.TryGetValue(gameObject.name, out var obj) && obj == gameObject)
            Instances.Remove(gameObject.name);
    }

    private void OnApplicationQuit()
    {
        Instances.Clear();
    }
}
