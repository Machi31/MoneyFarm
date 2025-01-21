using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Market : MonoBehaviour
{
    public static Market InstanceMarket { get; private set; }

    [SerializeField] private Transform[] _positions; 
    [SerializeField] private int _preselectId = 0;
    [SerializeField] private int _selectId = 0;
    [SerializeField] private float _smoothSpeed = 0.125f;
    private bool _isDragging;

    [SerializeField] private GameObject _farmWindow;
    [SerializeField] private GameObject _marketWindow;

    [SerializeField] private Image _bgFarms;
    [SerializeField] private Image _bgProfile;

    [SerializeField] private TMP_Text[] _countText;
    [SerializeField] private int[] _countProduct;
    public int[] _costProduct;

    [SerializeField] private TMP_Text[] _countMoneyText;
    [SerializeField] private int[] _countMoney; 

    private Coroutine[] _addCountMoneyIenumerator;

    private float _fadeDuaration = 0.5f;

    private void Start() {
        _addCountMoneyIenumerator = new Coroutine[_countMoney.Length];
        if (!GameManager.Instance._isFirst){
            _costProduct = PlayerPrefsX.GetIntArray("CostProduct");
            _countProduct = PlayerPrefsX.GetIntArray("CountProductMarket");
            _countMoney = PlayerPrefsX.GetIntArray("CountMoneyMarket");
        }
        else
            SaveData();

        int timesToAddMoney = Mathf.RoundToInt(GameManager.Instance._secondsFromExit / 5);
        for (int i = 0; i < _countProduct.Length; i++){
            if (_countProduct[i] > 0){
                if (timesToAddMoney > _countProduct[i]){
                    timesToAddMoney = _countProduct[i];
                    _countProduct[i] = 0;
                    _countMoney[i]  = _costProduct[i] * _countProduct[i];
                }
                else{
                    int countProduct = _countProduct[i] - timesToAddMoney;
                    _countProduct[i] = countProduct;
                    if (_countProduct[i] > 0){
                        if (_addCountMoneyIenumerator[i] == null)
                            _addCountMoneyIenumerator[i] = StartCoroutine(AddCountMoney(i));
                    }
                    _countMoney[i] = _costProduct[i] * timesToAddMoney;
                }
            }
        }

        PlayerPrefsX.SetIntArray("CountProductMarket", _countProduct);
    }

    private void Awake() {
        if (InstanceMarket != null && InstanceMarket != this)
        {
            Destroy(gameObject);
            return;
        }

        InstanceMarket = this;
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

    private void OnEnable(){
        MethodsFarm.OpenMarket += OpenMarket;
        Warehouse.SendToMarket += PlusProduct;
    }

    private void OnDisable(){
        MethodsFarm.OpenMarket -= OpenMarket;
        Warehouse.SendToMarket -= PlusProduct;
    }

    private void OpenMarket(){
        StartCoroutine(FadeOut());
        _farmWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _marketWindow.transform.DOMove(new Vector3(0, 0, 0), _fadeDuaration);
        UpdateShelf();
    }

    private void UpdateShelf(){
        for (int i = 0; i < _countProduct.Length; i++){
            if (_countProduct[i] <= 0){
                _countText[i].gameObject.SetActive(false);
                if (_addCountMoneyIenumerator[i] != null)
                    StopCoroutine(_addCountMoneyIenumerator[i]);
            }
            else{
                _countText[i].gameObject.SetActive(true);
                _countText[i].text = $"{_countProduct[i]}";
                if (_addCountMoneyIenumerator[i] == null)
                    _addCountMoneyIenumerator[i] = StartCoroutine(AddCountMoney(i));
            }
            _countMoneyText[i].text = $"{_countMoney[i]}";
        }
    }

    public void CloseMarket(){
        StartCoroutine(FadeOut());
        _farmWindow.transform.DOMove(new Vector3(0, 0, 0), _fadeDuaration);
        _marketWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
    }

    private void PlusProduct(int id, int count){
        _countProduct[id] += count;
        UpdateShelf();
        SaveData();
    }

    public void CollectMoney(int id){
        if (id < 6){
            MoneyAndGems.InstanceMG.PlusMoney(_countMoney[id]);
            _countMoney[id] = 0;
            UpdateShelf();
            SaveData();
        }
        else{
            MoneyAndGems.InstanceMG.PlusGem(_countMoney[id]);
            _countMoney[id] = 0;
            UpdateShelf();
            SaveData();
        }
    }

    private void SaveData() {
        PlayerPrefsX.SetIntArray("CostProduct", _costProduct);
        PlayerPrefsX.SetIntArray("CountProductMarket", _countProduct);
        PlayerPrefsX.SetIntArray("CountMoneyMarket", _countMoney);
    }

    private IEnumerator AddCountMoney(int id){
        while(true){
            yield return new WaitForSeconds(5);
            _countProduct[id] --;
            _countMoney[id] += _costProduct[id];
            UpdateShelf();
            SaveData();
        }
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        Color bgFarmsColor = _bgFarms.color;
        float bgFarmsStartAlpha = bgFarmsColor.a;

        Color bgProfileColor = _bgProfile.color;
        float bgProfileStartAlpha = bgProfileColor.a;

        if (bgFarmsStartAlpha == 1){
            while (elapsedTime < _fadeDuaration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / _fadeDuaration;

                bgFarmsColor.a = Mathf.Lerp(bgFarmsStartAlpha, 0f, t);
                _bgFarms.color = bgFarmsColor;

                bgProfileColor.a = Mathf.Lerp(bgProfileStartAlpha, 1f, t);
                _bgProfile.color = bgProfileColor;

                yield return null;
            }
            bgFarmsColor.a = 0f;
            _bgFarms.color = bgFarmsColor;

            bgProfileColor.a = 1f;
            _bgProfile.color = bgProfileColor;
        }
        else{
            while (elapsedTime < _fadeDuaration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / _fadeDuaration;

                bgFarmsColor.a = Mathf.Lerp(bgFarmsStartAlpha, 1f, t);
                _bgFarms.color = bgFarmsColor;

                bgProfileColor.a = Mathf.Lerp(bgProfileStartAlpha, 0f, t);
                _bgProfile.color = bgProfileColor;

                yield return null;
            }
            bgFarmsColor.a = 1f;
            _bgFarms.color = bgFarmsColor;

            bgProfileColor.a = 0f;
            _bgProfile.color = bgProfileColor;
        }
    }
}
