using System.Collections;
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MethodsFarm : MonoBehaviour
{
    public static event Action<int> AddWater;
    public static event Action<int> CollectProduct;
    public static event Action<int> UpgradeMaxProduct;
    public static event Action<int> UpgradeSpeedProduct;
    public static event Action OpenProfile;
    public static event Action OpenWarehouse;
    public static event Action OpenMarket;

    [SerializeField] private int _selectedId;

    [SerializeField] private GameObject _farmWindow;
    [SerializeField] private GameObject _profileWindow;
    [SerializeField] private GameObject _warehouseWindow;
    [SerializeField] private GameObject _marketWindow;

    [SerializeField] private Image _bgFarms;
    [SerializeField] private Image _bgProfile;

    [SerializeField] private Button[] _profileButton;

    private float _fadeDuaration = 0.5f;

    private void OnEnable(){
        SelectFarm.SetSelectedId += UpdateSelectedId;
    }

    private void OnDisable(){
        SelectFarm.SetSelectedId -= UpdateSelectedId;
    }

    private void UpdateSelectedId(int id) => _selectedId = id;
  
    public void AddWaterMethod(){
        if (_selectedId < 7)
            AddWater?.Invoke(_selectedId);
    }

    public void CollectProductMethod(){
        if (_selectedId < 7)
            CollectProduct?.Invoke(_selectedId);
    }

    public void UpgradeMaxProductFarm(){
        if (_selectedId < 7)
            UpgradeMaxProduct?.Invoke(_selectedId);
    }

    public void UpgradeSpeedProductFarm(){
        if (_selectedId < 7)
            UpgradeSpeedProduct?.Invoke(_selectedId);
    }

    public void OpenProfileMethod(){
        _farmWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _warehouseWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _marketWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        OpenProfile?.Invoke();
    }

    public void OpenWarehouseMethod(){
        _farmWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _profileWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _marketWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        OpenWarehouse?.Invoke();
        for (int i = 0; i < _profileButton.Length; i++){
            _profileButton[i].gameObject.SetActive(false);
            _profileButton[i].interactable = false;
        }
    }

    public void OpenMarketMethod(){
        _farmWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _profileWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _warehouseWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        OpenMarket?.Invoke();
        for (int i = 0; i < _profileButton.Length; i++){
            _profileButton[i].gameObject.SetActive(false);
            _profileButton[i].interactable = false;
        }
    }

    public void OpenFarm(){
        _farmWindow.transform.DOMove(new Vector3(0, 0, 0), _fadeDuaration);
        _profileWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _warehouseWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _marketWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        Color bgFarmsColor = _bgFarms.color;
        float bgFarmsStartAlpha = bgFarmsColor.a;

        Color bgProfileColor = _bgProfile.color;
        float bgProfileStartAlpha = bgProfileColor.a;

        while (elapsedTime < _fadeDuaration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _fadeDuaration;

            bgFarmsColor.a = Mathf.Lerp(bgFarmsStartAlpha, 1f, t);
            _bgFarms.color = bgFarmsColor;

            bgProfileColor.a = Mathf.Lerp(bgProfileStartAlpha, 0f, t);
            _bgProfile.color = bgProfileColor;
            for (int i = 0; i < _profileButton.Length; i++){
                _profileButton[i].gameObject.SetActive(false);
                _profileButton[i].interactable = false;
            }

            yield return null;
        }
        bgFarmsColor.a = 1f;
        _bgFarms.color = bgFarmsColor;

        bgProfileColor.a = 0f;
        _bgProfile.color = bgProfileColor;
    }
}
