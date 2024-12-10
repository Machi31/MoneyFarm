using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class BuyFarm : MonoBehaviour
{
    public static event Action<int> BuyNewFarmEvent;

    [SerializeField] private ProductFarm _productFarm;

    [SerializeField] private GameObject _buyFarmPanel;

    [SerializeField] private PackSeedlings[] _seedlings; 
    [SerializeField] private TMP_Text[] _lvlFarmText;
    [SerializeField] private int[] _lvlFarm;

    private int _selectedId;

    private void Start(){
        if (GameManager.Instance._isFirst)
        for (int i = 0; i < _lvlFarmText.Length; i++)
            _lvlFarmText[i].text = _lvlFarm[i].ToString();
    }

    private void OnEnable(){
        MethodsProfile.BuyFarm += ShowBuyFarmPanel;    
    }
    private void OnDisable(){
        MethodsProfile.BuyFarm -= ShowBuyFarmPanel;    
    }

    private void ShowBuyFarmPanel(int id){
        _buyFarmPanel.transform.DOMove(new Vector3(0, 0, 0), 0.5f);
        _selectedId = id;
        _seedlings[_selectedId]._smallSeedlings.SetActive(true);
        _seedlings[_selectedId]._middleSeedlings.SetActive(true);
        _seedlings[_selectedId]._hugeSeedlings.SetActive(true);
        _seedlings[_selectedId]._farmSeedlings.SetActive(true);
    }

    public void CloseBuyFarmPanel(){
        _buyFarmPanel.transform.DOMove(new Vector3(0, -13f, 0), 0.5f);
        _seedlings[_selectedId]._smallSeedlings.SetActive(false);
        _seedlings[_selectedId]._middleSeedlings.SetActive(false);
        _seedlings[_selectedId]._hugeSeedlings.SetActive(false);
        _seedlings[_selectedId]._farmSeedlings.SetActive(false);
    }

    public void BuyNewFarm(int id){
        switch (id){
            case 0:
                _productFarm.maxProduct[_selectedId] += 100;
                _productFarm.multipleTime[_selectedId] += 0.01f;
                MoneyAndGems.InstanceMG.MinusMoney(100);
            break;
            case 1:
                _productFarm.maxProduct[_selectedId] += 500;
                _productFarm.multipleTime[_selectedId] += 0.05f;
                MoneyAndGems.InstanceMG.MinusMoney(450);
            break;
            case 2:
                _productFarm.maxProduct[_selectedId] += 1000;
                _productFarm.multipleTime[_selectedId] += 0.1f;
                MoneyAndGems.InstanceMG.MinusMoney(800);
            break;
            case 3:
                _productFarm.maxProduct[_selectedId] += 1000;
                _productFarm.multipleTime[_selectedId] += 0.15f;
                MoneyAndGems.InstanceMG.MinusGem(10);
            break;
        }
        _lvlFarm[_selectedId]++;
        _lvlFarmText[_selectedId].text = _lvlFarm[_selectedId].ToString();
        _buyFarmPanel.transform.DOMove(new Vector3(0, -13f, 0), 0.5f);
        _seedlings[_selectedId]._smallSeedlings.SetActive(false);
        _seedlings[_selectedId]._middleSeedlings.SetActive(false);
        _seedlings[_selectedId]._hugeSeedlings.SetActive(false);
        _seedlings[_selectedId]._farmSeedlings.SetActive(false);
        BuyNewFarmEvent?.Invoke(_selectedId);
    }
}

[System.Serializable]

public class PackSeedlings{
    public GameObject _smallSeedlings;
    public GameObject _middleSeedlings;
    public GameObject _hugeSeedlings;
    public GameObject _farmSeedlings;
}
