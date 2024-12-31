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
    [SerializeField] private TMP_Text[] _countText;

    private int _selectedId;
    
    private float _fadeDuaration = 0.5f;

    private void Start() {
        if (!GameManager.Instance._isFirst)
            _countProduct = PlayerPrefsX.GetIntArray("CountProduct");

        else
            PlayerPrefsX.SetIntArray("CountPeoduct", _countProduct);
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
            _countText[i].text = $"{_countProduct[i]}";
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
    }

    public void SelectId(int id){
        _selectedId = id;
        SendId?.Invoke(_selectedId, _countProduct[_selectedId]);
        _sendProductWindow.transform.DOMove(new Vector3(0, 0, 0), _fadeDuaration);
    }

    private void CollectProduct(int id, int count){
        if (id < 6){
            _countProduct[id] += count;
            PlayerPrefsX.SetIntArray("CountPeoduct", _countProduct);
        }
    }

    private void SendProduct(){
        SendToMarket?.Invoke(_selectedId, _countProduct[_selectedId]);
        _countProduct[_selectedId] = 0;
        _sendProductWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
    }

    private void OnApplicationQuit() {
        PlayerPrefsX.SetIntArray("CountProduct", _countProduct);
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
