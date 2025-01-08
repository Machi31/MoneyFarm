using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

//! ПЕРЕДЕЛАТЬ ЛОГИКУ ПРОДАЖИ ТОВАРА

public class Market : MonoBehaviour
{
    public static event Action<int, int, int> SendProductToBox;

    public static Market InstanceMarket { get; private set; }

    [SerializeField] private Transform[] _positions; 
    [SerializeField] private int _preselectId = 0;
    [SerializeField] private int _selectId = 0;
    [SerializeField] private float _smoothSpeed = 0.125f;
    private bool _isDragging;

    [SerializeField] private GameObject _sellProductWindow;

    [SerializeField] private GameObject _farmWindow;
    [SerializeField] private GameObject _marketWindow;

    [SerializeField] private Image _bgFarms;
    [SerializeField] private Image _bgProfile;

    [SerializeField] private int[] _countProduct;
    public int[] _costProduct;

    [SerializeField] private int _idNeedSell;
    [SerializeField] private int _needSell;

    private float _fadeDuaration = 0.5f;

    private void Start() {
        if (!GameManager.Instance._isFirst){
            _costProduct = PlayerPrefsX.GetIntArray("CostProduct");
            _countProduct = PlayerPrefsX.GetIntArray("CountProductMarket");
        }
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
        ButtonMarket.GiveFromMarket += NeedToSell;
    }

    private void OnDisable(){
        MethodsFarm.OpenMarket -= OpenMarket;
        Warehouse.SendToMarket -= PlusProduct;
        ButtonMarket.GiveFromMarket -= NeedToSell;
    }

    private void OpenMarket(){
        StartCoroutine(FadeOut());
        _farmWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _marketWindow.transform.DOMove(new Vector3(0, 0, 0), _fadeDuaration);
        UpdateShelf();
    }

    private void UpdateShelf(){
        
    }

    public void CloseMarket(){
        StartCoroutine(FadeOut());
        _farmWindow.transform.DOMove(new Vector3(0, 0, 0), _fadeDuaration);
        _marketWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
    }

    private void PlusProduct(int id, int count){
        _countProduct[id] += count;
    }
    public void MinusProduct(){
        _countProduct[_idNeedSell] -= _needSell;
        if (_idNeedSell < 6)
            MoneyAndGems.InstanceMG.PlusMoney(_needSell * _costProduct[_idNeedSell]);
        else
            MoneyAndGems.InstanceMG.PlusGem(_needSell * _costProduct[_idNeedSell]);
        _sellProductWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        UpdateShelf();
    }

    private void NeedToSell(int id, int count){
        _idNeedSell = id;
        _needSell = count;
    } 

    public void BreakSell() => _sellProductWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);

    private void OnApplicationQuit() {
        PlayerPrefsX.SetIntArray("CostProduct", _costProduct);
        PlayerPrefsX.SetIntArray("CountProductMarket", _countProduct);
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
