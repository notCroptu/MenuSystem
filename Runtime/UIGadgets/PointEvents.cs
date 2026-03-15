using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class PointerEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Button assignIfButtonCantClick;
    [SerializeField] private Toggle assignIfToggleCantClick;
    public UnityEvent onEnter;
    public UnityEvent onClick;
    public UnityEvent onExit;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (assignIfButtonCantClick != null && !assignIfButtonCantClick.interactable) return;
        if (assignIfToggleCantClick != null && !assignIfToggleCantClick.interactable) return;
        onClick?.Invoke();

        if (assignIfButtonCantClick != null)
            assignIfButtonCantClick.onClick.Invoke();
        if (assignIfToggleCantClick != null)
            assignIfToggleCantClick.onValueChanged.Invoke(assignIfToggleCantClick.isOn);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (assignIfButtonCantClick != null && !assignIfButtonCantClick.interactable) return;
        if (assignIfToggleCantClick != null && !assignIfToggleCantClick.interactable) return;

        Debug.Log("Hovered played sound from: " + name + " " + transform.parent.name);

        onEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (assignIfButtonCantClick != null && !assignIfButtonCantClick.interactable) return;
        if (assignIfToggleCantClick != null && !assignIfToggleCantClick.interactable) return;

        onExit?.Invoke();
    }
}