using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesFarm : MonoBehaviour
{
    public static event Action<int> UpgradeMaxProductFarm;

    [SerializeField] private ProductFarm _productFarm;
    [SerializeField] private BuyFarm _buyFarm;

    [SerializeField] private Button _fullWaterButton;
    [SerializeField] private Button _collectAllButton;

    private int _selectedId;

    private void OnEnable(){
        MethodsFarm.UpgradeMaxProduct += UpgradeMaxProduct;
        MethodsFarm.UpgradeSpeedProduct += UpgradeSpeedProductFarm;
        SelectFarm.SetSelectedId += UpdateSelectedId;
    }

    private void OnDisable(){
        MethodsFarm.UpgradeMaxProduct -= UpgradeMaxProduct;
        MethodsFarm.UpgradeSpeedProduct -= UpgradeSpeedProductFarm;
        SelectFarm.SetSelectedId -= UpdateSelectedId;
    }

    private void Update(){
        if (_selectedId < 7 && _buyFarm._lvlFarm[_selectedId] > 0){
            if (MoneyAndGems.InstanceMG.money < 150){
                _fullWaterButton.interactable = false;
                _collectAllButton.interactable = false;
            }
            else {
                _fullWaterButton.interactable = true;
                _collectAllButton.interactable = true;
            }
        }
    }

    private void UpdateSelectedId(int id) => _selectedId = id;

    private void UpgradeMaxProduct(int id){
        MoneyAndGems.InstanceMG.money -= 150;
        _productFarm.maxProduct[id] += 50;
        UpgradeMaxProductFarm?.Invoke(id);
        MoneyAndGems.InstanceMG.MinusMoney(50);
    }

    private void UpgradeSpeedProductFarm(int id){
        MoneyAndGems.InstanceMG.money -= 150;
        _productFarm.multipleTime[id] += 0.1f;
        _productFarm.ColculateTimeToPlus(id);
        MoneyAndGems.InstanceMG.MinusMoney(500);
    }
}
