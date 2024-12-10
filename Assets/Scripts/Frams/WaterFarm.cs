using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaterFarm : MonoBehaviour
{
    public static event Action<int> UpdateFarm;

    [SerializeField] private Image _image;
    [SerializeField] private float _fadeDuration = 1f;

    [SerializeField] private TMP_Text _waterText;
    [SerializeField] private int _selectedId;
    [SerializeField] private int _timeToMinusWater;
    public int[] percentWater;


    private Coroutine[] _minusWater;
    private Coroutine _onlyFadeCoroutine;
    private Coroutine _onlyOutCoroutine;

    private void Start(){
        _minusWater = new Coroutine[percentWater.Length];
        _waterText.text = $"{percentWater[_selectedId]} / 100%";
        for (int i = 0; i < percentWater.Length; i++){
            if (percentWater[i] > 0)
                _minusWater[i] = StartCoroutine(MinusWater(i));
        }
    }

    private void OnEnable(){
        MethodsFarm.AddWater += AddWater;
        SelectFarm.SetSelectedId += UpdateSelectedId;
    }

    private void OnDisable(){
        MethodsFarm.AddWater -= AddWater;
        SelectFarm.SetSelectedId -= UpdateSelectedId;
    }

    private void UpdateSelectedId(int id){
        _selectedId = id;
        if (_selectedId < 7){
            _waterText.text = $"{percentWater[_selectedId]} / 100%";
            if (_onlyOutCoroutine != null)
                StopCoroutine(_onlyOutCoroutine);
                    
            _onlyOutCoroutine = StartCoroutine(OnlyOut());
        }
        else{
            _waterText.text = "";
            if (_onlyFadeCoroutine != null)
                StopCoroutine(_onlyFadeCoroutine);

            _onlyFadeCoroutine = StartCoroutine(OnlyFade());
        }
    }

    private void AddWater(int id){
        if (percentWater[_selectedId] == 0){
            percentWater[_selectedId] += 10;
            _waterText.text = $"{percentWater[_selectedId]} / 100%";
            _minusWater[id] = StartCoroutine(MinusWater(id));
        }
        else{
            percentWater[_selectedId] += 10;
            _waterText.text = $"{percentWater[_selectedId]} / 100%";
        }
        UpdateFarm?.Invoke(id);
    }

    private IEnumerator MinusWater(int id){
        yield return new WaitForSeconds(_timeToMinusWater);

        percentWater[id]--;
            if (percentWater[id] == 0){
                if (id == _selectedId)
                    _waterText.text = $"0 / 100%";
                UpdateFarm?.Invoke(id);
            }
            else{
                if (id == _selectedId)
                    _waterText.text = $"{percentWater[id]} / 100%";
                _minusWater[id] = StartCoroutine(MinusWater(id));
            }
    }

    private IEnumerator OnlyFade(){

        float elapsedTime = 0f;

        Color selectColor = _image.color;
        float selectStartAlpha = selectColor.a;

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _fadeDuration;

            selectColor.a = Mathf.Lerp(selectStartAlpha, 0f, t);
            _image.color = selectColor;

            yield return null;
        }

        selectColor.a = 0f;
        _image.color = selectColor;
    }

    private IEnumerator OnlyOut()
    {

        float elapsedTime = 0f;

        Color selectColor = _image.color;
        float selectStartAlpha = selectColor.a;

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _fadeDuration;

            selectColor.a = Mathf.Lerp(selectStartAlpha, 1f, t);
            _image.color = selectColor;

            yield return null;
        }

        selectColor.a = 1f;
        _image.color = selectColor;
    }
}
