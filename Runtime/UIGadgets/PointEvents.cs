using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// Component must be in same game object as ui object to use hover sound
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class PointEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler , IPointerClickHandler// Maybe script like button audio that is used like raycasts instead for objects (maybe not needed because sound effect can be used in conjunction with custom interaction scripts to achieve that)
{
    [SerializeField] private UnityAction _press;
    [SerializeField] private UnityAction _hover;
    [SerializeField] private UnityAction _unHover;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _hover?.Invoke();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _unHover?.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _press?.Invoke();
    }

}
