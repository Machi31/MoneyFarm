using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickerPlus : MonoBehaviour
{
    public static event Action<int> ClickPlusProduct;

    [SerializeField] private WaterFarm _waterFarm;

    [SerializeField] private Button _button;
    [SerializeField] private int _selectedId; 

    private void OnEnable() {
        SelectFarm.SetSelectedId += UpdateSelectedId;
    }
    private void OnDisable() {
        SelectFarm.SetSelectedId += UpdateSelectedId;
    }

    private void Update() {
        if (_selectedId < 7 && _waterFarm.percentWater[_selectedId] > 0)
            _button.interactable = true;
        else
            _button.interactable = false;
    }

    private void UpdateSelectedId(int id){
        _selectedId = id;
    }

    public void ClickPlusProductMethod(){
        ClickPlusProduct?.Invoke(_selectedId);
    }
}
