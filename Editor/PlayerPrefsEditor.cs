using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

public class PlayerPrefsEditor
{
    [MenuItem("Tools/Clear PlayerPrefs")]
    public static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs cleared!");
    }
}

#endif