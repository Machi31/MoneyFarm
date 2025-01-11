using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyFarm : MonoBehaviour
{
    public static event Action<int> BuyNewFarmEvent;
    public static event Action BuyWater;
    public static event Action BuyKultivator;

    [SerializeField] private ProductFarm _productFarm;

    [SerializeField] private GameObject _buyFarmPanel;
    [SerializeField] private GameObject _buyBonuspanel;
    [SerializeField] private GameObject[] _bonusFarms;

    [SerializeField] private PackSeedlings[] _seedlings;
    [SerializeField] private Button _buyBonusBtn;
    [SerializeField] private Button[] _buyBtn;
    [SerializeField] private TMP_Text[] _lvlFarmText;
    public int[] _lvlFarm;

    private int _selectedId;

    [SerializeField] private GameObject[] _true;
    [SerializeField] private GameObject[] _false;
    private bool _autoCollect;
    private bool _autoPlusWater;

    private void Start(){
        _autoCollect = PlayerPrefsX.GetBool("AutoCollect", false);
        _autoPlusWater = PlayerPrefsX.GetBool("AutoWaterPlus", false);
        if (!GameManager.Instance._isFirst)
            _lvlFarm = PlayerPrefsX.GetIntArray("LvlFarm");

        for (int i = 0; i < _lvlFarmText.Length; i++)
            _lvlFarmText[i].text = _lvlFarm[i].ToString();
        
        if (!_autoCollect){
            _true[0].SetActive(false);
            _false[0].SetActive(true);
        }
        else{
            _true[0].SetActive(true);
            _false[0].SetActive(false);
        }

        if (!_autoPlusWater){
            _true[1].SetActive(false);
            _false[1].SetActive(true);
        }
        else{
            _true[1].SetActive(true);
            _false[1].SetActive(false);
        }
    }

    private void OnEnable(){
        MethodsProfile.BuyFarm += ShowBuyFarmPanel;    
    }
    private void OnDisable(){
        MethodsProfile.BuyFarm -= ShowBuyFarmPanel;    
    }

    public void ShowBuyFarmPanel(int id){
        _selectedId = id;
        if (id < 6 || id == 6 && _lvlFarm[id] > 0){
            _buyFarmPanel.transform.DOMove(new Vector3(0, 0, 0), 0.5f);
            _buyBonuspanel.transform.DOMove(new Vector3(0, -15, 0), 0.5f);
            for (int i = 0; i < _buyBtn.Length; i++){
                switch (i){
                    case 0:
                        if (MoneyAndGems.InstanceMG.money < 100)
                            _buyBtn[0].interactable = false;
                        else
                            _buyBtn[0].interactable = true;
                    break;
                    case 1:
                        if (MoneyAndGems.InstanceMG.money < 450)
                            _buyBtn[1].interactable = false;
                        else
                            _buyBtn[1].interactable = true;
                    break;
                    case 2:
                        if (MoneyAndGems.InstanceMG.money < 800)
                            _buyBtn[2].interactable = false;
                        else
                            _buyBtn[2].interactable = true;
                    break;
                    case 3:
                        if (MoneyAndGems.InstanceMG.gems < 10)
                            _buyBtn[3].interactable = false;
                        else
                            _buyBtn[3].interactable = true;
                    break;
                }
            }
            _seedlings[_selectedId]._smallSeedlings.SetActive(true);
            _seedlings[_selectedId]._middleSeedlings.SetActive(true);
            _seedlings[_selectedId]._hugeSeedlings.SetActive(true);
            _seedlings[_selectedId]._farmSeedlings.SetActive(true);
        }
        else{
            _buyBonuspanel.transform.DOMove(new Vector3(0, 0, 0), 0.5f);
            _buyFarmPanel.transform.DOMove(new Vector3(0, -15, 0), 0.5f);

            _bonusFarms[id - 6].SetActive(true);
            if (MoneyAndGems.InstanceMG.gems < 500)
                _buyBonusBtn.interactable = false;
            else
                _buyBonusBtn.interactable = true;
        }
    }

    public void CloseBuyFarmPanel(){
        _buyFarmPanel.transform.DOMove(new Vector3(0, -15, 0), 0.5f);
        _buyBonuspanel.transform.DOMove(new Vector3(0, -15, 0), 0.5f);
        if (_selectedId < 6){
            _seedlings[_selectedId]._smallSeedlings.SetActive(false);
            _seedlings[_selectedId]._middleSeedlings.SetActive(false);
            _seedlings[_selectedId]._hugeSeedlings.SetActive(false);
            _seedlings[_selectedId]._farmSeedlings.SetActive(false);
        }
        else{
            for (int i = 0; i < 3; i++)
                _bonusFarms[i].SetActive(false);
        }
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
        _buyFarmPanel.transform.DOMove(new Vector3(0, -15, 0), 0.5f);
        _buyBonuspanel.transform.DOMove(new Vector3(0, -15, 0), 0.5f);
        _seedlings[_selectedId]._smallSeedlings.SetActive(false);
        _seedlings[_selectedId]._middleSeedlings.SetActive(false);
        _seedlings[_selectedId]._hugeSeedlings.SetActive(false);
        _seedlings[_selectedId]._farmSeedlings.SetActive(false);
        BuyNewFarmEvent?.Invoke(_selectedId);
        SaveData();
    }

    public void BuyBonus(){
        MoneyAndGems.InstanceMG.MinusGem(500);
        switch (_selectedId){
            case 6:
                _lvlFarm[6]++;
                _lvlFarmText[6].text = _lvlFarm[6].ToString();
            break;
            case 7:
                BuyKultivator?.Invoke();
                _autoCollect = true;
            break;
            case 8:
                BuyWater?.Invoke();
                _autoPlusWater = true;
            break;
        }
        _buyFarmPanel.transform.DOMove(new Vector3(0, -15, 0), 0.5f);
        _buyBonuspanel.transform.DOMove(new Vector3(0, -15, 0), 0.5f);
        for (int i = 0; i < 3; i++)
            _bonusFarms[i].SetActive(false);
        
        if (!_autoCollect){
            _true[0].SetActive(false);
            _false[0].SetActive(true);
        }
        else{
            _true[0].SetActive(true);
            _false[0].SetActive(false);
        }

        if (!_autoPlusWater){
            _true[1].SetActive(false);
            _false[1].SetActive(true);
        }
        else{
            _true[1].SetActive(true);
            _false[1].SetActive(false);
        }
        SaveData();
    }

    private void SaveData() {
        PlayerPrefsX.SetIntArray("LvlFarm", _lvlFarm);
    }
}

[System.Serializable]

public class PackSeedlings{
    public GameObject _smallSeedlings;
    public GameObject _middleSeedlings;
    public GameObject _hugeSeedlings;
    public GameObject _farmSeedlings;
}
