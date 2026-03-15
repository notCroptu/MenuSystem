using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ButtonVisuals : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image _visualImage;
    [SerializeField] private Button _button;
    [SerializeField] private Sprite _normalSprite;
    [SerializeField] private Sprite _hoverSprite;
    [SerializeField] private Sprite _pressedSprite;
    [SerializeField] private Sprite _disabledSprite;
    public UnityEvent onEnter;
    public UnityEvent onClick;


    private void Awake()
    {
        _button = GetComponent<Button>();
        UpdateSprite();
    }

    private void Update()
    {
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (_button != null && !_button.interactable)
        {
            _visualImage.sprite = _disabledSprite;
        }
        else if (_visualImage.sprite == _disabledSprite)
        {
            _visualImage.sprite = _normalSprite;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_button == null || !_button.interactable) return;
        _visualImage.sprite = _hoverSprite;
        onEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_button == null || !_button.interactable) return;
        _visualImage.sprite = _normalSprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_button == null || !_button.interactable) return;
        _visualImage.sprite = _pressedSprite;
        onClick?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_button == null || !_button.interactable) return;
        _visualImage.sprite = _hoverSprite;
    }
}
