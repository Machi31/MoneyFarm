using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Warehouse : MonoBehaviour
{
    public static event Action<int, int> SendToMarket;
    public static event Action<int, int> SendId;

    [SerializeField] private GameObject _farmWindow;
    [SerializeField] private GameObject _warehouseWindow;
    [SerializeField] private GameObject _sendProductWindow;

    [SerializeField] private Image _bgFarms;
    [SerializeField] private Image _bgProfile;

    [SerializeField] private int[] _countProduct;
    [SerializeField] private TMP_Text[] _costText;
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private TMP_Text _costSendText;

    [SerializeField] private Scrollbar _scrollbar;
    private int _sendProduct;

    private int _selectedId;
    
    private float _fadeDuaration = 0.5f;

    private bool _isOpened = false;

    private void Start() {
        if (!GameManager.Instance._isFirst)
            _countProduct = PlayerPrefsX.GetIntArray("CountProductWarehouse");

        else
            PlayerPrefsX.SetIntArray("CountProductWarehouse", _countProduct);
    }

    private void Update() {
        if (_isOpened){
            _sendProduct = (int)Math.Floor(_scrollbar.value * _countProduct[_selectedId]);
            _costSendText.text = $"{_sendProduct * Market.InstanceMarket._costProduct[_selectedId]}";
        }
    }

    private void OnEnable(){
        ProductFarm.CollectFarm += CollectProduct;
        MethodsFarm.OpenWarehouse += OpenWarehouse;
        MethodsWarehouse.SendProductMethod += SendProduct;
    }

    private void OnDisable(){
        ProductFarm.CollectFarm -= CollectProduct;
        MethodsFarm.OpenWarehouse -= OpenWarehouse;
        MethodsWarehouse.SendProductMethod -= SendProduct;
    }

    private void OpenWarehouse(){
        StartCoroutine(FadeOut());
        for (int i = 0; i < _countProduct.Length; i++)
            _costText[i].text = $"{Market.InstanceMarket._costProduct[i]}";
        _farmWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _warehouseWindow.transform.DOMove(new Vector3(0, 0, 0), _fadeDuaration);
    }

    public void CloseWarehouse(){
        StartCoroutine(FadeOut());
        _farmWindow.transform.DOMove(new Vector3(0, 0, 0), _fadeDuaration);
        _warehouseWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
    }

    public void CloseSendWarehouse(){
        _sendProductWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _isOpened = false;
    }

    public void SelectId(int id){
        _isOpened = true;
        _selectedId = id;
        SendId?.Invoke(_selectedId, _countProduct[_selectedId]);
        _countText.text = $"{_countProduct[id]}";
        _sendProductWindow.transform.DOMove(new Vector3(0, 0, 0), _fadeDuaration);
    }

    private void CollectProduct(int id, int count){
        if (id < 6){
            _countProduct[id] += count;
            PlayerPrefsX.SetIntArray("CountProductWarehouse", _countProduct);
            SaveData();
        }
    }

    private void SendProduct(){
        SendToMarket?.Invoke(_selectedId, _sendProduct);
        _countProduct[_selectedId] -= _sendProduct;
        _countText.text = $"{_countProduct[_selectedId]}";
        _scrollbar.value = 0;
        _sendProductWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _isOpened = false;
        SaveData();
    }

    private void SaveData() {
        PlayerPrefsX.SetIntArray("CountProductWarehouse", _countProduct);
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
