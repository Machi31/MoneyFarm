using System.Collections;
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using GamePush;

public class MethodsFarm : MonoBehaviour
{
    public static event Action<int> AddWater;
    public static event Action<int> CollectProduct;
    public static event Action<int> UpgradeMaxProduct;
    public static event Action<int> UpgradeMaxProductBonus;
    public static event Action<int> OpenFullProfile;
    public static event Action OpenProfile;
    public static event Action OpenWarehouse;
    public static event Action OpenMarket;
    public static event Action OpenGemMarket;
    public static event Action CollectAll;
    public static event Action WaterFull;
    public static event Action PlusSpeed;
    public static event Action SlowWater;
    public static event Action PlusMaxSpeed;

    [SerializeField] private int _selectedId;

    [SerializeField] private GameObject _farmWindow;
    [SerializeField] private GameObject _profileWindow;
    [SerializeField] private GameObject _warehouseWindow;
    [SerializeField] private GameObject _marketWindow;
    [SerializeField] private GameObject _gemMarketWindow;
    [SerializeField] private GameObject _upgradeMaxCountWindow;
    [SerializeField] private GameObject _upgradeSpeedWindow;

    [SerializeField] private Image _bgFarms;
    [SerializeField] private Image _bgProfile;

    // [SerializeField] private Button[] _profileButton;
    [SerializeField] private Button _fullWaterButton;
    [SerializeField] private Button _collectAllButton;
    [SerializeField] private Button _plusSpeedButton;
    [SerializeField] private Button _slowWaterButton;
    [SerializeField] private Button _upgradeMaxCountButton;

    private float _fadeDuaration = 0.5f;

    private void OnEnable(){
        SelectFarm.SetSelectedId += UpdateSelectedId;
        GP_Ads.OnRewardedReward += OnRewarded;
    }

    private void OnDisable(){
        SelectFarm.SetSelectedId -= UpdateSelectedId;
        GP_Ads.OnRewardedReward -= OnRewarded;
    }

    private void Update() {
        if (MoneyAndGems.InstanceMG.gems < 10){
            _plusSpeedButton.interactable = false;
            _slowWaterButton.interactable = false;
            _upgradeMaxCountButton.interactable = false;
        }
        else {
            _plusSpeedButton.interactable = true;
            _slowWaterButton.interactable = true;
            _upgradeMaxCountButton.interactable = true;
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

    public void UpgradeMaxCount(){
        if (_selectedId < 7)
            UpgradeMaxProduct?.Invoke(_selectedId);
    }

    public void UpgradeMaxCountBonus(){
        MoneyAndGems.InstanceMG.MinusGem(10);
        if (_selectedId < 7)
            UpgradeMaxProductBonus?.Invoke(_selectedId);
        OpenFarm();
    }

    public void UpgradeSpeed(){
        GP_Ads.ShowRewarded("SpeedReward");
    }

    public void UpgradeMaxCountFarm(){
        _upgradeMaxCountWindow.transform.DOMove(new Vector3(0, 1, 0), _fadeDuaration);
        _upgradeSpeedWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _farmWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _profileWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _warehouseWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _marketWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _gemMarketWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
    }

    public void UpgradeSpeedProductFarm(){
        _upgradeSpeedWindow.transform.DOMove(new Vector3(0, 1, 0), _fadeDuaration);
        _upgradeMaxCountWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _farmWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _profileWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _warehouseWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _marketWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _gemMarketWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
    }

    public void OpenProfileMethod(){
        _farmWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _warehouseWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _marketWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _gemMarketWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _upgradeMaxCountWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _upgradeSpeedWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        OpenProfile?.Invoke();
    }

    public void OpenWarehouseMethod(){
        _farmWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _profileWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _marketWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _gemMarketWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _upgradeMaxCountWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _upgradeSpeedWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        OpenWarehouse?.Invoke();
    }

    public void OpenMarketMethod(){
        _farmWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _profileWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _warehouseWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _gemMarketWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _upgradeMaxCountWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _upgradeSpeedWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        OpenMarket?.Invoke();
    }

    public void OpenGemMarketMethod(){
        _farmWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _profileWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _warehouseWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _marketWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _upgradeMaxCountWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _upgradeSpeedWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        OpenGemMarket?.Invoke();
    }

    public void OpenUpgradeFarmWindowMethod(){
        _farmWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _warehouseWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _marketWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _gemMarketWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _upgradeMaxCountWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _upgradeSpeedWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        OpenFullProfile?.Invoke(_selectedId);
    }

    public void OpenFarm(){
        _farmWindow.transform.DOMove(new Vector3(0, 0, 0), _fadeDuaration);
        _profileWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _warehouseWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _marketWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _gemMarketWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _upgradeMaxCountWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        _upgradeSpeedWindow.transform.DOMove(new Vector3(0, -15, 0), _fadeDuaration);
        StartCoroutine(FadeOut());
    }

    public void CollectAllMethod(){
        GP_Ads.ShowRewarded("CollectAll");
    }
    public void WaterFullMethod(){
        GP_Ads.ShowRewarded("WaterFull");
    }
    public void PlusSpeedMethod(){
        MoneyAndGems.InstanceMG.gems -= 10;
        PlusSpeed?.Invoke();
    }
    public void SlowWaterMethod(){
        MoneyAndGems.InstanceMG.gems -= 10;
        SlowWater?.Invoke();
    }

    private void OnRewarded(string arg0){
        if (arg0 == "SpeedReward")
            PlusMaxSpeed?.Invoke();
        else if (arg0 == "CollectAll")
            CollectAll?.Invoke();
        else if (arg0 == "WaterFull")
            WaterFull?.Invoke();
        OpenFarm();
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
