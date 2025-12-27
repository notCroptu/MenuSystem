using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreditsInspector : MonoBehaviour
{
    [field:SerializeField] public bool AllowSkip { get; private set; } = true;
    // serialized cuz i was debugging
    [field: SerializeField] public bool LetDrag { get; private set; } = false;
    [SerializeField] private  Image _background;
    [SerializeField] [Range(0f,1f)] private float _minAlpha = 0f;
    [SerializeField] [Range(0f,1f)] private float _maxAlpha = 0.75f;
    [SerializeField] private  bool _goBack = false;
    [SerializeField] private float _scrollTime = 30f;
    [SerializeField] private Credits _credits;
    [SerializeField] private GameObject _gameObject;

    private Coroutine _cor;

    private void OnEnable()
    {
        _credits.verticalNormalizedPosition = 1f;

        StartCoroutine(Fade(true));
    }

    private void Update()
    {
        if ( LetDrag ) return;
        
        if ( _credits.verticalNormalizedPosition > 0f )
            _credits.verticalNormalizedPosition -= Mathf.Max(0f, Time.deltaTime/ _scrollTime );
        else
        {
            if ( _cor == null)
                _cor = StartCoroutine(Fade(false));
        }
    }

    private IEnumerator Fade(bool inOrOut)
    {
        float initAlpha = _minAlpha;
        float finalAlpha = _maxAlpha;
        float current = initAlpha;

        Debug.Log("in: " + initAlpha + " out: " + finalAlpha + " delta: " + Time.deltaTime / 0.8f * Mathf.Sign(finalAlpha - initAlpha));

        Color newColor = _background.color;

        do
        {
            current += Time.deltaTime / 0.8f;
            newColor.a = Mathf.SmoothStep(initAlpha, finalAlpha, current);
            if (!inOrOut)
                newColor.a = _maxAlpha - newColor.a;
            _background.color = newColor;

            yield return null;
        }
        while ( newColor.a < _maxAlpha && newColor.a > _minAlpha );

        newColor.a = finalAlpha;
        _background.color = newColor;

        if (!inOrOut)
        {
            _gameObject.SetActive(false);
            if ( _goBack )
                SceneManager.LoadScene("MainMenu");
        }

        _cor = null;
    }

    public void SetDrag(bool dragging)
    {
        Debug.Log("Setting drag. ");
        LetDrag = dragging;
    }
}
