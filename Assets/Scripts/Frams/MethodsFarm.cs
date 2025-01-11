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
    public static event Action OpenProfile;
    public static event Action OpenWarehouse;
    public static event Action OpenMarket;
    public static event Action OpenGemMarket;
    public static event Action CollectAll;
    public static event Action WaterFull;
    public static event Action PlusSpeed;
    public static event Action SlowWater;

    [SerializeField] private int _selectedId;

    [SerializeField] private GameObject _farmWindow;
    [SerializeField] private GameObject _profileWindow;
    [SerializeField] private GameObject _warehouseWindow;
    [SerializeField] private GameObject _marketWindow;
    [SerializeField] private GameObject _gemMarketWindow;

    [SerializeField] private Image _bgFarms;
    [SerializeField] private Image _bgProfile;

    // [SerializeField] private Button[] _profileButton;
    [SerializeField] private Button _fullWaterButton;
    [SerializeField] private Button _collectAllButton;
    [SerializeField] private Button _plusSpeedButton;
    [SerializeField] private Button _slowWaterButton;

    private float _fadeDuaration = 0.5f;

    private void OnEnable(){
        SelectFarm.SetSelectedId += UpdateSelectedId;
    }

    private void OnDisable(){
        SelectFarm.SetSelectedId -= UpdateSelectedId;
    }

    private void Update() {
        if (MoneyAndGems.InstanceMG.money < 500){
            _fullWaterButton.interactable = false;
            _collectAllButton.interactable = false;
        }
        else {
            _fullWaterButton.interactable = true;
            _collectAllButton.interactable = true;
        }

        if (MoneyAndGems.InstanceMG.gems < 10){
            _plusSpeedButton.interactable = false;
            _slowWaterButton.interactable = false;
        }
        else {
            _plusSpeedButton.interactable = true;
            _slowWaterButton.interactable = true;
        }
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
        
    }

    public void OpenProfileMethod(){
        _farmWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _warehouseWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _marketWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _gemMarketWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        OpenProfile?.Invoke();
    }

    public void OpenWarehouseMethod(){
        _farmWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _profileWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _marketWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _gemMarketWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        OpenWarehouse?.Invoke();
        // for (int i = 0; i < _profileButton.Length; i++){
        //     _profileButton[i].gameObject.SetActive(false);
        //     _profileButton[i].interactable = false;
        // }
    }

    public void OpenMarketMethod(){
        _farmWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _profileWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _warehouseWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _gemMarketWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        OpenMarket?.Invoke();
        // for (int i = 0; i < _profileButton.Length; i++){
        //     _profileButton[i].gameObject.SetActive(false);
        //     _profileButton[i].interactable = false;
        // }
    }

    public void OpenGemMarketMethod(){
        _farmWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _profileWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _warehouseWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _marketWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        OpenGemMarket?.Invoke();
        // for (int i = 0; i < _profileButton.Length; i++){
        //     _profileButton[i].gameObject.SetActive(false);
        //     _profileButton[i].interactable = false;
        // }
    }

    public void OpenFarm(){
        _farmWindow.transform.DOMove(new Vector3(0, 0, 0), _fadeDuaration);
        _profileWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _warehouseWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _marketWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _gemMarketWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        StartCoroutine(FadeOut());
    }

    public void CollectAllMethod(){
        MoneyAndGems.InstanceMG.money -= 500;
        CollectAll?.Invoke();
    }
    public void WaterFullMethod(){
        MoneyAndGems.InstanceMG.money -= 500;
        WaterFull?.Invoke();
    }
    public void PlusSpeedMethod(){
        MoneyAndGems.InstanceMG.gems -= 10;
        PlusSpeed?.Invoke();
    }
    public void SlowWaterMethod(){
        MoneyAndGems.InstanceMG.gems -= 10;
        SlowWater?.Invoke();
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
            // for (int i = 0; i < _profileButton.Length; i++){
            //     _profileButton[i].gameObject.SetActive(false);
            //     _profileButton[i].interactable = false;
            // }

            yield return null;
        }
        bgFarmsColor.a = 1f;
        _bgFarms.color = bgFarmsColor;

        bgProfileColor.a = 0f;
        _bgProfile.color = bgProfileColor;
    }
}
