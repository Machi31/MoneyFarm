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
    [SerializeField] private CostsFarms[] _costsFarm;
    [SerializeField] private Button _buyBonusBtn;
    [SerializeField] private Button[] _buyBtn;
    [SerializeField] private GameObject _gem;
    [SerializeField] private GameObject _coin;
    [SerializeField] private TMP_Text[] _lvlFarmText;
    [SerializeField] private TMP_Text _costNewFarmText;
    [SerializeField] private TMP_Text _costSmallFarmText;
    [SerializeField] private TMP_Text _costMediumFarmText;
    [SerializeField] private TMP_Text _costHighFarmText;
    [SerializeField] private TMP_Text _costLargeFarmText;
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
        else
            SaveData();

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
        if (_lvlFarm[id] > 0){
            _buyFarmPanel.transform.DOMove(new Vector3(0, 0, 0), 0.5f);
            _buyBonuspanel.transform.DOMove(new Vector3(0, -15, 0), 0.5f);
            _costSmallFarmText.text = _costsFarm[_selectedId]._costSmallFarm.ToString();
            _costMediumFarmText.text = _costsFarm[_selectedId]._costMediumFarm.ToString();
            _costHighFarmText.text = _costsFarm[_selectedId]._costHighFarm.ToString();
            _costLargeFarmText.text = _costsFarm[_selectedId]._costLargeFarm.ToString();
            for (int i = 0; i < _buyBtn.Length; i++){
                switch (i){
                    case 0:
                        if (MoneyAndGems.InstanceMG.money < _costsFarm[_selectedId]._costSmallFarm)
                            _buyBtn[0].interactable = false;
                        else
                            _buyBtn[0].interactable = true;
                    break;
                    case 1:
                        if (MoneyAndGems.InstanceMG.money < _costsFarm[_selectedId]._costMediumFarm)
                            _buyBtn[1].interactable = false;
                        else
                            _buyBtn[1].interactable = true;
                    break;
                    case 2:
                        if (MoneyAndGems.InstanceMG.money < _costsFarm[_selectedId]._costHighFarm)
                            _buyBtn[2].interactable = false;
                        else
                            _buyBtn[2].interactable = true;
                    break;
                    case 3:
                        if (MoneyAndGems.InstanceMG.gems < _costsFarm[_selectedId]._costLargeFarm)
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

            if (_selectedId >= 6){
                _coin.SetActive(false);
                _gem.SetActive(true);
                _bonusFarms[id - 6].SetActive(true);
                _costNewFarmText.text = $"Купить за 500";
                if (MoneyAndGems.InstanceMG.gems < 500)
                    _buyBonusBtn.interactable = false;
                else
                    _buyBonusBtn.interactable = true;
            }
            else{
                _gem.SetActive(false);
                _coin.SetActive(true);
                _seedlings[_selectedId]._newFarmSeedlings.SetActive(true);
                _costNewFarmText.text = $"Купить за {_costsFarm[_selectedId]._costNewFarm.ToString()}";
                if (MoneyAndGems.InstanceMG.money < _costsFarm[_selectedId]._costNewFarm)
                    _buyBonusBtn.interactable = false;
                else
                    _buyBonusBtn.interactable = true;
            }
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
            _seedlings[_selectedId]._newFarmSeedlings.SetActive(false);
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
                    MoneyAndGems.InstanceMG.MinusMoney(_costsFarm[_selectedId]._costSmallFarm);
                break;
                case 1:
                    _productFarm.maxProduct[_selectedId] += 300;
                    _productFarm.multipleTime[_selectedId] += 0.05f;
                    MoneyAndGems.InstanceMG.MinusMoney(_costsFarm[_selectedId]._costMediumFarm);
                break;
                case 2:
                    _productFarm.maxProduct[_selectedId] += 800;
                    _productFarm.multipleTime[_selectedId] += 0.1f;
                    MoneyAndGems.InstanceMG.MinusMoney(_costsFarm[_selectedId]._costHighFarm);
                break;
                case 3:
                    _productFarm.maxProduct[_selectedId] += 1000;
                    _productFarm.multipleTime[_selectedId] += 0.15f;
                    MoneyAndGems.InstanceMG.MinusGem(_costsFarm[_selectedId]._costLargeFarm);
                break;
            }
        _lvlFarm[_selectedId]++;
        _lvlFarmText[_selectedId].text = _lvlFarm[_selectedId].ToString();
        CloseBuyFarmPanel();
        BuyNewFarmEvent?.Invoke(_selectedId);
        SaveData();
    }

    public void BuyBonus(){
        switch (_selectedId){
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
                MoneyAndGems.InstanceMG.MinusMoney(_costsFarm[_selectedId]._costSmallFarm);
                _lvlFarm[_selectedId]++;
                _lvlFarmText[_selectedId].text = _lvlFarm[_selectedId].ToString();
                BuyNewFarmEvent?.Invoke(_selectedId);
            break;
            case 6:
                MoneyAndGems.InstanceMG.MinusGem(500);
                _lvlFarm[6]++;
                _lvlFarmText[6].text = _lvlFarm[6].ToString();
            break;
            case 7:
                MoneyAndGems.InstanceMG.MinusGem(500);
                BuyKultivator?.Invoke();
                _autoCollect = true;
            break;
            case 8:
                MoneyAndGems.InstanceMG.MinusGem(500);
                BuyWater?.Invoke();
                _autoPlusWater = true;
            break;
        }
        CloseBuyFarmPanel();
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
    public GameObject _newFarmSeedlings;
}

[System.Serializable]

public class CostsFarms{
    public int _costNewFarm;
    public int _costSmallFarm;
    public int _costMediumFarm;
    public int _costHighFarm;
    public int _costLargeFarm;
}
