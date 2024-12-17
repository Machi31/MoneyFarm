using UnityEngine;

public class VisualFarm : MonoBehaviour
{
    [SerializeField] private WaterFarm _waterFarm;
    [SerializeField] private ProductFarm _moneyFarm;

    [SerializeField] private Farms[] _farms;

    private bool _autoCollect;
    private bool _autoPlusWater;
    
    private void Start() {
        _autoCollect = PlayerPrefsX.GetBool("AutoCollect", false);
        _autoPlusWater = PlayerPrefsX.GetBool("AutoWaterPlus", false);
        
        for (int i = 0; i < _farms.Length; i++){
            for (int j = 0; j < _farms[i]._farms.Length; j++)
                _farms[i]._farms[j].SetActive(false);
            
            if (i <= 6){
                if (_waterFarm.percentWater[i] == 0)
                    _farms[i]._farms[0].SetActive(true);
                else if (_moneyFarm.product[i] >= _moneyFarm.maxProduct[i] / 10)
                    _farms[i]._farms[2].SetActive(true);
                else
                    _farms[i]._farms[1].SetActive(true);
            }
            else{
                if (i == 7 && _autoCollect)
                    _farms[i]._farms[2].SetActive(true);
                else if (i == 7 && !_autoCollect)
                    _farms[i]._farms[0].SetActive(true);
                if (i == 8 && _autoPlusWater)
                    _farms[i]._farms[1].SetActive(true);
                else if (i == 8 && !_autoPlusWater)
                    _farms[i]._farms[0].SetActive(true);
            }
        }
    }

    private void OnEnable() {
        WaterFarm.UpdateFarm += UpdateFarm;
        ProductFarm.UpdateFarm += UpdateFarm;
        BuyFarm.BuyWater += AutoWater;
        BuyFarm.BuyKultivator += AutoCollect;
    }

    private void OnDisable() {
        WaterFarm.UpdateFarm -= UpdateFarm;
        ProductFarm.UpdateFarm -= UpdateFarm;
        BuyFarm.BuyWater -= AutoWater;
        BuyFarm.BuyKultivator -= AutoCollect;
    }

    private void AutoCollect() {
        _autoCollect = true;
        UpdateBonus();
    } 
    private void AutoWater(){
        _autoPlusWater = true;        
        UpdateBonus();
    } 

    private void UpdateFarm(int id){
        for (int j = 0; j < _farms[id]._farms.Length; j++)
            _farms[id]._farms[j].SetActive(false);
        
        if (_waterFarm.percentWater[id] == 0)
            _farms[id]._farms[0].SetActive(true);
        else if (_moneyFarm.product[id] >= _moneyFarm.maxProduct[id] / 10)
            _farms[id]._farms[2].SetActive(true);
        else
            _farms[id]._farms[1].SetActive(true);
    }

    private void UpdateBonus(){
        for (int i = 7; i < _farms.Length; i++){
            for (int j = 0; j < _farms[i]._farms.Length; j++)
                _farms[i]._farms[j].SetActive(false);
            
            if (i == 7 && _autoCollect)
                    _farms[i]._farms[2].SetActive(true);
            else if (i == 7 && !_autoCollect)
                _farms[i]._farms[0].SetActive(true);
            if (i == 8 && _autoPlusWater)
                _farms[i]._farms[1].SetActive(true);
            else if (i == 8 && !_autoPlusWater)
                _farms[i]._farms[0].SetActive(true);
        }
    }
}

[System.Serializable]
public class Farms{
    public GameObject[] _farms;
}
