using UnityEngine;
using UnityEngine.UI;

public static class ContentSizeUpdate
{
    public static void ForceUpdateAllCanvases()
    {
        Canvas[] canvases = Object.FindObjectsByType<Canvas>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (Canvas canvas in canvases)
        {
            RectTransform[] rects = canvas.GetComponentsInChildren<RectTransform>(true);
            foreach (RectTransform rect in rects)
            {
                if (rect.GetComponent<LayoutGroup>() || rect.GetComponent<ContentSizeFitter>())
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
                    Debug.Log("Rebuilding " + rect.gameObject.name);
                }
            }
        }
    }
}