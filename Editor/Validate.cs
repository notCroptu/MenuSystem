using UnityEngine;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;

public static class Validate
{
    [MenuItem("Tools/Run OnValidate On All ScriptableObjects")]
    public static void RunOnValidate()
    {
        string[] guids = AssetDatabase.FindAssets("t:ScriptableObject");
        int count = 0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ScriptableObject obj = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);

            if (obj != null)
            {
                MethodInfo validate = obj.GetType()
                    .GetMethod("OnValidate", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                if (validate != null)
                {
                    validate.Invoke(obj, null);
                    count++;
                }
            }
        }

        Debug.Log($"Manually invoked OnValidate() on {count} ScriptableObjects.");
    }
}
#endif
