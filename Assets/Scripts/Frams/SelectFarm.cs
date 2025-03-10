using System;
using System.Collections;
using TMPro;
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

    [SerializeField] private GameObject _coin;
    [SerializeField] private GameObject _gem;
    [SerializeField] private TMP_Text _costText;
    [SerializeField] private Button _buyFarmButton;
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
        _buyFarmButton.gameObject.SetActive(false);
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
                        if (_buyFarm._lvlFarm[_selectId] > 0){
                            _buyFarmButton.gameObject.SetActive(false);
                            _farmsButton[j].gameObject.SetActive(true);
                            _farmsButton[j].interactable = _buyFarm._lvlFarm[_selectId] > 0;
                        }
                        else{
                            _farmsButton[j].gameObject.SetActive(false);
                            _buyFarmButton.gameObject.SetActive(true);
                            _costText.text = $"Купить за {_buyFarm._costsFarm[_selectId]._costNewFarm}";
                            if (_selectId < 6){
                                _coin.SetActive(true);
                                _gem.SetActive(false);
                            }
                            else{
                                _gem.SetActive(true);
                                _coin.SetActive(false);
                            }
                        }
                    }
                    for (int j = 0; j < _waterButton.Length; j++)
                        _waterButton[j].gameObject.SetActive(false);
                    for (int j = 0; j < _kultivatorButton.Length; j++)
                        _kultivatorButton[j].gameObject.SetActive(false);
                }
                else if (_selectId == 7){
                    _buyFarmButton.gameObject.SetActive(false);
                    for (int j = 0; j < _farmsButton.Length; j++)
                        _farmsButton[j].gameObject.SetActive(false);
                    for (int j = 0; j < _waterButton.Length; j++)
                        _waterButton[j].gameObject.SetActive(false);
                    for (int j = 0; j < _kultivatorButton.Length; j++)
                        _kultivatorButton[j].gameObject.SetActive(true);
                }
                else if (_selectId == 8){
                    _buyFarmButton.gameObject.SetActive(false);
                    for (int j = 0; j < _farmsButton.Length; j++)
                        _farmsButton[j].gameObject.SetActive(false);
                    for (int j = 0; j < _kultivatorButton.Length; j++)
                        _kultivatorButton[j].gameObject.SetActive(false);
                    for (int j = 0; j < _waterButton.Length; j++)
                        _waterButton[j].gameObject.SetActive(true);
                }
                

                if (_selectId < 7 && _preselectId != 7){
                    if (_fadeCoroutine != null)
                        StopAllCoroutines();

                    _fadeCoroutine = StartCoroutine(FadeOut(_selectId, _preselectId));
                }
                else if (_selectId == 7 && _preselectId < 7){
                    if (_onlyFadeCoroutine != null)
                        StopAllCoroutines();

                    _onlyFadeCoroutine = StartCoroutine(OnlyFade(_selectId, _preselectId));
                }
                else if (_selectId == 6 && _preselectId == 7){
                    if (_onlyOutCoroutine != null)
                        StopAllCoroutines();
                    
                    _onlyOutCoroutine = StartCoroutine(OnlyOut(_selectId, _preselectId));
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

    private IEnumerator FadeOut(int id, int preId)
    {
        for (int i = 0; i < _image.Length; i++){
            if (i != id && i != preId){
                Color color = _image[i].color;
                color.a = 0;
                _image[i].color = color;
            }
        }

        float elapsedTime = 0f;

        Color preselectColor = _image[preId].color;
        float preselectStartAlpha = preselectColor.a;

        Color selectColor = _image[id].color;
        float selectStartAlpha = selectColor.a;

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _fadeDuration;

            preselectColor.a = Mathf.Lerp(preselectStartAlpha, 0f, t);
            _image[preId].color = preselectColor;

            selectColor.a = Mathf.Lerp(selectStartAlpha, 1f, t);
            _image[id].color = selectColor;

            yield return null;
        }

        preselectColor.a = 0f;
        _image[preId].color = preselectColor;

        selectColor.a = 1f;
        _image[id].color = selectColor;
    }

    private IEnumerator OnlyFade(int id, int preId){
        for (int i = 0; i < _image.Length; i++){
            if (i != id && i != preId){
                Color color = _image[i].color;
                color.a = 0;
                _image[i].color = color;
            }
        }

        float elapsedTime = 0f;

        Color preselectColor = _image[preId].color;
        float preselectStartAlpha = preselectColor.a;

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _fadeDuration;

            preselectColor.a = Mathf.Lerp(preselectStartAlpha, 0f, t);
            _image[preId].color = preselectColor;

            yield return null;
        }

        preselectColor.a = 0f;
        _image[preId].color = preselectColor;
    }

    private IEnumerator OnlyOut(int id, int preId)
    {
        for (int i = 0; i < _image.Length; i++){
            if (i != id && i != preId){
                Color color = _image[i].color;
                color.a = 0;
                _image[i].color = color;
            }
        }

        float elapsedTime = 0f;

        Color selectColor = _image[id].color;
        float selectStartAlpha = selectColor.a;

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _fadeDuration;

            selectColor.a = Mathf.Lerp(selectStartAlpha, 1f, t);
            _image[id].color = selectColor;

            yield return null;
        }

        selectColor.a = 1f;
        _image[id].color = selectColor;
    }
}
