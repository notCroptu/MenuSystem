using TMPro;
using UnityEngine;

public class VersionText : MonoBehaviour
{
    [SerializeField] private TMP_Text versionText;

    private void Awake()
    {
        versionText.text = $"v {Application.version}";
    }
}
