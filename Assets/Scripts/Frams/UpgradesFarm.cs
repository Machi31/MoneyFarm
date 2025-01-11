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

    [SerializeField] private Button _upgradeFullButton;

    private int _selectedId;

    private void OnEnable(){
        MethodsFarm.UpgradeMaxProduct += UpgradeMaxProduct;
        SelectFarm.SetSelectedId += UpdateSelectedId;
    }

    private void OnDisable(){
        MethodsFarm.UpgradeMaxProduct -= UpgradeMaxProduct;
        SelectFarm.SetSelectedId -= UpdateSelectedId;
    }

    private void Update(){
        if (_selectedId < 7 && _buyFarm._lvlFarm[_selectedId] > 0){
            if (MoneyAndGems.InstanceMG.money < 10){
                _upgradeFullButton.interactable = false;
            }
            else {
                _upgradeFullButton.interactable = true;
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
}
