using UnityEngine;
using UnityEngine.UI;

public class FindObject : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private string _tag;

    private void Awake()
    {
        _button?.onClick.AddListener(FindAndEnableObject);
    }

    public GameObject FindAndReturnObject()
    {
        GameObject go = GameObject.FindWithTag(_tag);

        if (go == null)
            Debug.LogWarning("Could NOT find an object with tag: " + _tag);

        if (go != null)
        {
            go.transform.GetChild(0)?.gameObject.SetActive(true);
            return go;
        }

        return null;
    }

    public void FindAndEnableObject()
    {
        GameObject go = GameObject.FindWithTag(_tag);

        if (go == null)
            Debug.LogWarning("Could NOT find an object with tag: " + _tag);

        if (go != null)
        {
            go.transform.GetChild(0)?.gameObject.SetActive(true);
        }
    }
}
