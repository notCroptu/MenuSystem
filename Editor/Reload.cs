
#if UNITY_EDITOR
using UnityEditor;

public static class Reload
{
    [MenuItem("Tools/Force Script Reload")]
    public static void ForceReload()
    {
        AssetDatabase.Refresh();
        UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
    }
}
#endif
