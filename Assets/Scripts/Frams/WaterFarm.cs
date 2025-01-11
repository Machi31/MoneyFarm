using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaterFarm : MonoBehaviour
{
    public static event Action<int> UpdateFarm;
    public static event Action<int, float, int> SendNeedTime;

    [SerializeField] private Image _image;
    [SerializeField] private float _fadeDuration = 1f;

    [SerializeField] private TMP_Text _waterText;
    [SerializeField] private int _selectedId;
    [SerializeField] private float _timeToMinusWater;
    public int[] percentWater;

    private bool _autoWaterPlus;

    private Coroutine[] _minusWater;
    private Coroutine _onlyFadeCoroutine;
    private Coroutine _onlyOutCoroutine;

    private void Start(){
        _autoWaterPlus = PlayerPrefsX.GetBool("AutoWaterPlus", false);
        _minusWater = new Coroutine[percentWater.Length];
        if (!GameManager.Instance._isFirst)
            percentWater = PlayerPrefsX.GetIntArray("PercentWater");

        float maxTime;
        for (int i = 0; i < percentWater.Length; i++){
            maxTime = percentWater[i] * _timeToMinusWater;
            if (maxTime > GameManager.Instance._secondsFromExit)
                maxTime -= GameManager.Instance._secondsFromExit;
            else
                maxTime = 0f;
            int prePercent = percentWater[i];
            percentWater[i] -= Mathf.RoundToInt(prePercent - Math.Abs(maxTime / _timeToMinusWater));
            if (percentWater[i] < 0)
                percentWater[i] = 0;
            int minuspercent = prePercent - percentWater[i];
            if (minuspercent > 0)
                SendNeedTime?.Invoke(i, minuspercent * _timeToMinusWater, percentWater[i]);
            else if (minuspercent == 0 && prePercent != 0)
                SendNeedTime?.Invoke(i, GameManager.Instance._secondsFromExit, percentWater[i]);
            if (percentWater[i] < 50 && _autoWaterPlus)
                percentWater[i] = 100;
            
            UpdateFarm?.Invoke(i);
        }

        _waterText.text = $"{percentWater[_selectedId]} / 100%";
        for (int i = 0; i < percentWater.Length; i++){
            if (percentWater[i] > 0)
                _minusWater[i] = StartCoroutine(MinusWater(i));
        }
    }

    private void OnEnable(){
        MethodsFarm.AddWater += AddWater;
        MethodsFarm.WaterFull += AddFullWater;
        MethodsFarm.SlowWater += SlowWater;
        SelectFarm.SetSelectedId += UpdateSelectedId;
        BuyFarm.BuyWater += AutoWater;
    }

    private void OnDisable(){
        MethodsFarm.AddWater -= AddWater;
        MethodsFarm.WaterFull -= AddFullWater;
        MethodsFarm.SlowWater -= SlowWater;
        SelectFarm.SetSelectedId -= UpdateSelectedId;
        BuyFarm.BuyWater -= AutoWater;
    }

    private void AutoWater() => _autoWaterPlus = true;

    private void AddFullWater(){
        for (int i = 0; i < percentWater.Length; i++){
            percentWater[i] = 100;
            _waterText.text = $"{percentWater[_selectedId]} / 100%";
        }
        SaveData();
    }

    private void SlowWater(){
        _timeToMinusWater += 10;
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
            _minusWater[id] = StartCoroutine(MinusWater(id));
        }
        else if (percentWater[_selectedId] + 10 < 100)
            percentWater[_selectedId] += 10;
        else
            percentWater[_selectedId] = 100;

        _waterText.text = $"{percentWater[_selectedId]} / 100%";
        UpdateFarm?.Invoke(id);
        SaveData();
    }

    private void SaveData() {
        PlayerPrefsX.SetIntArray("PercentWater", percentWater);
        PlayerPrefsX.SetBool("AutoWaterPlus", _autoWaterPlus);
    }

    private IEnumerator MinusWater(int id){
        yield return new WaitForSeconds(_timeToMinusWater);

        percentWater[id]--;
        if (_autoWaterPlus && percentWater[id] < 50){
            percentWater[id] = 100;
            if (id == _selectedId)
                _waterText.text = $"{percentWater[id]} / 100%";
            _minusWater[id] = StartCoroutine(MinusWater(id));
        }
        else{
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
        SaveData();
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
