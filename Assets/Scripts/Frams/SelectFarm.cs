using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SelectFarm : MonoBehaviour
{
    public static event Action<int> SetSelectedId;

    [SerializeField] private BuyFarm _buyFarm;

    [SerializeField] private Image[] _image;
    [SerializeField] private float _fadeDuration = 1f;

    [SerializeField] private Transform[] _positions; 
    [SerializeField] private int _preselectId = 0;
    [SerializeField] private int _selectId = 0;

    [SerializeField] private Button[] _farmsButton;
    [SerializeField] private Button[] _waterButton;
    [SerializeField] private Button[] _kultivatorButton;

    [SerializeField] private float _smoothSpeed = 0.125f;
    private bool _isDragging;

    private Coroutine _fadeCoroutine;
    private Coroutine _onlyFadeCoroutine;
    private Coroutine _onlyOutCoroutine;

    private void Start()
    {
        for (int i = 0; i < _image.Length; i++)
        {
            Color initialColor = _image[i].color;
            initialColor.a = (i == _selectId) ? 1f : 0f;
            _image[i].color = initialColor;
        }
        for (int i = 0; i < _farmsButton.Length; i++)
            _farmsButton[i].interactable = _buyFarm._lvlFarm[_selectId] > 0;
        for (int j = 0; j < _waterButton.Length; j++)
            _waterButton[j].gameObject.SetActive(false);
        for (int j = 0; j < _kultivatorButton.Length; j++)
            _kultivatorButton[j].gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_positions.Length == 0) return;

        float minDistance = Vector3.Distance(transform.position, _positions[_selectId].position);

        for (int i = 0; i < _positions.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, _positions[i].position);

            if (distance < minDistance)
            {
                minDistance = distance;
                _preselectId = _selectId;
                _selectId = i;
                SetSelectedId?.Invoke(_selectId);

                if (_selectId < 7){
                    for (int j = 0; j < _farmsButton.Length; j++){
                        _farmsButton[j].gameObject.SetActive(true);
                        _farmsButton[j].interactable = _buyFarm._lvlFarm[_selectId] > 0;
                    }
                    for (int j = 0; j < _waterButton.Length; j++)
                        _waterButton[j].gameObject.SetActive(false);
                    for (int j = 0; j < _kultivatorButton.Length; j++)
                        _kultivatorButton[j].gameObject.SetActive(false);
                }
                else if (_selectId == 7){
                    for (int j = 0; j < _farmsButton.Length; j++)
                        _farmsButton[j].gameObject.SetActive(false);
                    for (int j = 0; j < _waterButton.Length; j++)
                        _waterButton[j].gameObject.SetActive(false);
                    for (int j = 0; j < _kultivatorButton.Length; j++)
                        _kultivatorButton[j].gameObject.SetActive(true);
                }
                else if (_selectId == 8){
                    for (int j = 0; j < _farmsButton.Length; j++)
                        _farmsButton[j].gameObject.SetActive(false);
                    for (int j = 0; j < _kultivatorButton.Length; j++)
                        _kultivatorButton[j].gameObject.SetActive(false);
                    for (int j = 0; j < _waterButton.Length; j++)
                        _waterButton[j].gameObject.SetActive(true);
                }
                

                if (_selectId < 7 && _preselectId != 7){
                    if (_fadeCoroutine != null)
                        StopCoroutine(_fadeCoroutine);

                    _fadeCoroutine = StartCoroutine(FadeOut());
                }
                else if (_selectId == 7 && _preselectId < 7){
                    if (_onlyFadeCoroutine != null)
                        StopCoroutine(_onlyFadeCoroutine);

                    _onlyFadeCoroutine = StartCoroutine(OnlyFade());
                }
                else if (_selectId == 6 && _preselectId == 7){
                    if (_onlyOutCoroutine != null)
                        StopCoroutine(_onlyOutCoroutine);
                    
                    _onlyOutCoroutine = StartCoroutine(OnlyOut());
                }
            }
        }

        if (!_isDragging)
        {
            Vector3 desiredPosition = new (_positions[_selectId].position.x, _positions[_selectId].position.y, transform.position.z);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);

            transform.position = smoothedPosition;
        }
    }

    public void OnBeginDrag() => _isDragging = true;
    public void OnEndDrag() => _isDragging = false;

    private IEnumerator FadeOut()
    {
        for (int i = 0; i < _image.Length; i++){
            if (i != _selectId && i != _preselectId){
                Color color = _image[i].color;
                color.a = 0;
                _image[i].color = color;
            }
        }

        float elapsedTime = 0f;

        Color preselectColor = _image[_preselectId].color;
        float preselectStartAlpha = preselectColor.a;

        Color selectColor = _image[_selectId].color;
        float selectStartAlpha = selectColor.a;

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _fadeDuration;

            preselectColor.a = Mathf.Lerp(preselectStartAlpha, 0f, t);
            _image[_preselectId].color = preselectColor;

            selectColor.a = Mathf.Lerp(selectStartAlpha, 1f, t);
            _image[_selectId].color = selectColor;

            yield return null;
        }

        preselectColor.a = 0f;
        _image[_preselectId].color = preselectColor;

        selectColor.a = 1f;
        _image[_selectId].color = selectColor;
    }

    private IEnumerator OnlyFade(){
        for (int i = 0; i < _image.Length; i++){
            if (i != _selectId && i != _preselectId){
                Color color = _image[i].color;
                color.a = 0;
                _image[i].color = color;
            }
        }

        float elapsedTime = 0f;

        Color preselectColor = _image[_preselectId].color;
        float preselectStartAlpha = preselectColor.a;

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _fadeDuration;

            preselectColor.a = Mathf.Lerp(preselectStartAlpha, 0f, t);
            _image[_preselectId].color = preselectColor;

            yield return null;
        }

        preselectColor.a = 0f;
        _image[_preselectId].color = preselectColor;
    }

    private IEnumerator OnlyOut()
    {
        for (int i = 0; i < _image.Length; i++){
            if (i != _selectId && i != _preselectId){
                Color color = _image[i].color;
                color.a = 0;
                _image[i].color = color;
            }
        }

        float elapsedTime = 0f;

        Color selectColor = _image[_selectId].color;
        float selectStartAlpha = selectColor.a;

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _fadeDuration;

            selectColor.a = Mathf.Lerp(selectStartAlpha, 1f, t);
            _image[_selectId].color = selectColor;

            yield return null;
        }

        selectColor.a = 1f;
        _image[_selectId].color = selectColor;
    }
}
