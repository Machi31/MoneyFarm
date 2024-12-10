using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesFarm : MonoBehaviour
{
    public static event Action<int> UpgradeMaxProductFarm;

    [SerializeField] private ProductFarm _productFarm;

    private void OnEnable(){
        MethodsFarm.UpgradeMaxProduct += UpgradeMaxProduct;
        MethodsFarm.UpgradeSpeedProduct += UpgradeSpeedProductFarm;
    }

    private void OnDisable(){
        MethodsFarm.UpgradeMaxProduct -= UpgradeMaxProduct;
        MethodsFarm.UpgradeSpeedProduct -= UpgradeSpeedProductFarm;
    }

    private void UpgradeMaxProduct(int id){
        _productFarm.maxProduct[id] += 50;
        UpgradeMaxProductFarm?.Invoke(id);
        MoneyAndGems.InstanceMG.MinusMoney(50);
    }

    private void UpgradeSpeedProductFarm(int id){
        _productFarm.multipleTime[id] += 0.1f;
        _productFarm.ColculateTimeToPlus(id);
        MoneyAndGems.InstanceMG.MinusMoney(500);
    }
}
