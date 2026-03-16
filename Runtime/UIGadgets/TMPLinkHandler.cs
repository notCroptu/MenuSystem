using UnityEngine;
using TMPro;
using NaughtyAttributes;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TMP_Text))]
public class TMPLinkHandler : MonoBehaviour, IPointerClickHandler, IPointerMoveHandler, IPointerExitHandler
{
    [SerializeField] private bool _useOwnHoverColor = false;

    [ShowIf(nameof(_useOwnHoverColor))]
    [SerializeField] private Color _hoverColor = Color.cyan;
    
    private TMP_Text _textMeshPro;
    private int _hoveredLink = -1;

    private void Awake()
    {
        _textMeshPro = GetComponent<TMP_Text>();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        Camera cam = eventData.pressEventCamera;
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(
            _textMeshPro,
            eventData.position,
            cam
        );

        Color hoverColor;

        if (MenuData.HasInstance)
            hoverColor = MenuData.Instance.LinkHoverColor;
        else
            hoverColor = _hoverColor;

        if (linkIndex != _hoveredLink)
        {
            ClearHover();
            _hoveredLink = linkIndex;

            if (_hoveredLink != -1)
            {
                TMP_LinkInfo linkInfo = _textMeshPro.textInfo.linkInfo[linkIndex];

                for (int i = 0; i < linkInfo.linkTextLength; i++)
                {
                    int charIndex = linkInfo.linkTextfirstCharacterIndex + i;
                    TMP_CharacterInfo charInfo = _textMeshPro.textInfo.characterInfo[charIndex];

                    if (!charInfo.isVisible)
                        continue;

                    int matIndex = charInfo.materialReferenceIndex;
                    int vertIndex = charInfo.vertexIndex;

                    Color32[] colors = _textMeshPro.textInfo.meshInfo[matIndex].colors32;

                    colors[vertIndex + 0] = hoverColor;
                    colors[vertIndex + 1] = hoverColor;
                    colors[vertIndex + 2] = hoverColor;
                    colors[vertIndex + 3] = hoverColor;
                }

                _textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ClearHover();
        _hoveredLink = -1;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_hoveredLink == -1)
            return;

        TMP_LinkInfo linkInfo = _textMeshPro.textInfo.linkInfo[_hoveredLink];
        Application.OpenURL(linkInfo.GetLinkID());
    }

    private void ClearHover()
    {
        _textMeshPro.ForceMeshUpdate();
        _textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }
}
