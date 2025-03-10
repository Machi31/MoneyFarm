using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductFarm : MonoBehaviour
{
    public static event Action<int> UpdateFarm;
    public static event Action<int, int> CollectFarm;

    [SerializeField] private WaterFarm _waterFarm;
    [SerializeField] private BuyFarm _buyFarm;

    [SerializeField] private TMP_Text _EPHText;
    [SerializeField] private int _EPH;

    [SerializeField] private TMP_Text _productText;
    [SerializeField] private int _selectedId;
    public float[] timeToPlusProduct;
    public float[] multipleTime;
    public int[] maxProduct;
    public int[] product;

    [SerializeField] private Button _plusSpeedButton;
    [SerializeField] private TMP_Text _timeBonusText;
    private int _secondsFromExit;
    private int _timeBonus = 10800;
    private int _startNowTimeBonus;
    private int _nowTimeBonus;
    private int _collectTimes;

    private bool _isUpdated = false;

    private bool _autoCollect;

    private Coroutine[] _plusProductCor;

    [SerializeField] private Button _upgradeSpeedButton;

    private void Start(){
        _autoCollect = PlayerPrefsX.GetBool("AutoCollect", false);
        if (!GameManager.Instance._isFirst){
            _nowTimeBonus = PlayerPrefs.GetInt("NowTimeBonus");
            timeToPlusProduct = PlayerPrefsX.GetFloatArray("TimeToPlusProduct");
            multipleTime = PlayerPrefsX.GetFloatArray("MultipleTime");
            maxProduct = PlayerPrefsX.GetIntArray("MaxProduct");
            product = PlayerPrefsX.GetIntArray("Product");

            string exitTimeString = PlayerPrefs.GetString("ExitTime");
            DateTime exitTime = DateTime.Parse(exitTimeString);
            TimeSpan timeSinceExit = DateTime.Now - exitTime;
            _secondsFromExit = (int)timeSinceExit.TotalSeconds;
            if (_secondsFromExit < _nowTimeBonus){
                _startNowTimeBonus = _nowTimeBonus - _secondsFromExit;
                _nowTimeBonus -= _secondsFromExit;
                StartCoroutine(DisplayTimeRemaining());
                _plusSpeedButton.interactable = false;
            }
            else{
                _startNowTimeBonus = _nowTimeBonus;
                _nowTimeBonus = 0;
                _timeBonusText.text = "";
            }
        }
        else{
            timeToPlusProduct[0] = 36000 / 10000 / multipleTime[0];
            for (int i = 1; i < timeToPlusProduct.Length; i++)
                timeToPlusProduct[i] = timeToPlusProduct[0];
            SaveData();
        }
        _plusProductCor = new Coroutine[timeToPlusProduct.Length];
        _productText.text = $"{product[_selectedId]} / {maxProduct[_selectedId]}";
        UpdateEPH();
    }

    private void Update() {
        if (_nowTimeBonus > 0){
            _upgradeSpeedButton.interactable = false;
        }
        else {
            _upgradeSpeedButton.interactable = true;
        }
    }

    private void OnEnable(){
        MethodsFarm.CollectProduct += CollectProduct;
        MethodsFarm.CollectAll += CollectAll;
        MethodsFarm.PlusSpeed += PlusSpeed;
        MethodsFarm.PlusMaxSpeed += PlusSpeedMethod;
        MethodsFarm.UpgradeMaxProductBonus += PlusMaxCountBonus;
        WaterFarm.UpdateFarm += PlusProductFarm;
        WaterFarm.SendNeedTime += ColculateStartGame;
        SelectFarm.SetSelectedId += UpdateSelectedId;
        UpgradesFarm.UpgradeMaxProductFarm += UpdateSelectedId;
        BuyFarm.BuyNewFarmEvent += UpdateNewBuy;
        BuyFarm.BuyKultivator += AutoCollect;
        ClickerPlus.ClickPlusProduct += PlusProductClick;
    }

    private void OnDisable(){
        MethodsFarm.CollectProduct -= CollectProduct;
        MethodsFarm.CollectAll -= CollectAll;
        MethodsFarm.PlusSpeed -= PlusSpeed;
        MethodsFarm.PlusMaxSpeed -= PlusSpeedMethod;
        MethodsFarm.UpgradeMaxProductBonus -= PlusMaxCountBonus;
        WaterFarm.UpdateFarm -= PlusProductFarm;
        WaterFarm.SendNeedTime -= ColculateStartGame;
        SelectFarm.SetSelectedId -= UpdateSelectedId;
        UpgradesFarm.UpgradeMaxProductFarm -= UpdateSelectedId;
        BuyFarm.BuyNewFarmEvent -= UpdateNewBuy;
        BuyFarm.BuyKultivator -= AutoCollect;
        ClickerPlus.ClickPlusProduct -= PlusProductClick;
    }

    private void PlusProductClick(int id){
        if (product[id] < maxProduct[id] && _waterFarm.percentWater[id] > 0)
            product[id]++;
        _productText.text = $"{product[_selectedId]} / {maxProduct[_selectedId]}";
    }

    private void PlusMaxCountBonus(int id){
        maxProduct[id] += 100;
        _productText.text = $"{product[_selectedId]} / {maxProduct[_selectedId]}";
        SaveData();
    } 

    private void PlusSpeedMethod(){
        StopAllCoroutines();
        _nowTimeBonus = _timeBonus;
        for (int i = 0; i < timeToPlusProduct.Length; i++){
            ColculateTimeToPlus(i);
            PlusProductFarm(i);   
        }
        _plusSpeedButton.interactable = false;
        StartCoroutine(DisplayTimeRemaining());
    }

    private void AutoCollect() {
        _autoCollect = true;
        SaveData();
    }

    private void CollectAll(){
        for (int i = 0; i < product.Length; i++){
            CollectProduct(i);
            UpdateFarm?.Invoke(_selectedId);
        }
        SaveData();
    }

    private void PlusSpeed(){
        for (int i = 0; i < multipleTime.Length; i++){
            multipleTime[i] += 0.1f;
            ColculateTimeToPlus(i);
        }
        SaveData();
    }

    private void ColculateStartGame(int id, float time, int percentWater){
        if (_startNowTimeBonus > 0){
            if (_startNowTimeBonus > time){
                _startNowTimeBonus -= (int)time;
                _collectTimes += Mathf.RoundToInt(Math.Abs(time / timeToPlusProduct[id])) * 2;
            }
            else{
                _collectTimes += Mathf.RoundToInt(Math.Abs(_startNowTimeBonus / timeToPlusProduct[id])) * 2;
                time -= _startNowTimeBonus;
                if (time > 0)
                    _collectTimes += Mathf.RoundToInt(Math.Abs(time / timeToPlusProduct[id]));
            }
        }
        else{
            _collectTimes += Mathf.RoundToInt(Math.Abs(time / timeToPlusProduct[id]));
        }
        if (product[id] + _collectTimes < maxProduct[id])
            product[id] += _collectTimes;
        else
            product[id] = maxProduct[id];
        if (percentWater > 0 && product[id] < maxProduct[id])
            _plusProductCor[id] = StartCoroutine(PlusProductFarmCor(id));
        
        if (product[id] > maxProduct[id] / 2 && _autoCollect)
            CollectProduct(id);
        UpdateSelectedId(id);
    }

    private void UpdateSelectedId(int id){
        _selectedId = id;
        if (_selectedId < 7){
            _productText.text = $"{product[id]} / {maxProduct[id]}";
            ColculateTimeToPlus(id);
        }
        else{
            _productText.text = "";
        }
    }

    private void UpdateNewBuy(int id){
        if (id == _selectedId){
            if (_selectedId < 7){
                _productText.text = $"{product[id]} / {maxProduct[id]}";
                ColculateTimeToPlus(id);
            }
            else{
                _productText.text = "";
            }
        }
        UpdateEPH();
    }

    private void PlusProductFarm(int id){
        if (_waterFarm.percentWater[id] > 0 && product[id] < maxProduct[id]){
            if (_plusProductCor[id] != null)
                StopCoroutine(_plusProductCor[id]);
            _plusProductCor[id] = StartCoroutine(PlusProductFarmCor(id));
        }
        else{
            if (_plusProductCor[id] != null)
                StopCoroutine(_plusProductCor[id]);
        }
    }

    private void UpdateEPH(){
        _EPH = 0;
        for (int i = 0; i < timeToPlusProduct.Length; i++){
            if (_buyFarm._lvlFarm[i] > 0)
                _EPH +=  Mathf.RoundToInt(3600 / timeToPlusProduct[i] * Market.InstanceMarket._costProduct[i]);
        }
        _EPHText.text = _EPH.ToString();
    }

    public void ColculateTimeToPlus(int id){
        if (_nowTimeBonus > 0)
            timeToPlusProduct[id] = 36000 / 2 / 10000 / multipleTime[id];
        else
            timeToPlusProduct[id] = 36000 / 10000 / multipleTime[id];
        SaveData();
    }

    private void CollectProduct(int id){
        CollectFarm?.Invoke(id, product[id]);
        product[id] = 0;
        _productText.text = $"{product[id]} / {maxProduct[id]}";
        _isUpdated = false;
        UpdateFarm?.Invoke(_selectedId);
        if (_waterFarm.percentWater[id] > 0){
            if (_plusProductCor[id] != null)
                StopCoroutine(_plusProductCor[id]);
            _plusProductCor[id] = StartCoroutine(PlusProductFarmCor(id));
        }
        SaveData();
    }

    private void SaveData() {
        PlayerPrefs.SetInt("NowTimeBonus", _nowTimeBonus);
        PlayerPrefsX.SetIntArray("MaxProduct", maxProduct);
        PlayerPrefsX.SetIntArray("Product", product);
        PlayerPrefsX.SetFloatArray("MultipleTime", multipleTime);
        PlayerPrefsX.SetFloatArray("TimeToPlusProduct", timeToPlusProduct);
        PlayerPrefsX.SetBool("AutoCollect", _autoCollect);
    }

    private IEnumerator PlusProductFarmCor(int id){
        yield return new WaitForSeconds(timeToPlusProduct[id]);
        
        product[id]++;
        if (_autoCollect && product[id] > maxProduct[id] / 2){
            CollectProduct(id);
        }
        else{
            if (product[id] < maxProduct[id]){
                if (product[id] >= maxProduct[id] / 10 && !_isUpdated){
                    UpdateFarm?.Invoke(_selectedId);
                    _isUpdated = true;
                }

                if (_nowTimeBonus > 0 && _waterFarm.percentWater[id] > 0){
                    _plusProductCor[id] = StartCoroutine(PlusProductFarmCor(id));
                }
                else if (_waterFarm.percentWater[id] > 0){
                    ColculateTimeToPlus(id);
                    _plusProductCor[id] = StartCoroutine(PlusProductFarmCor(id));
                }
            }
            else
                UpdateFarm?.Invoke(_selectedId);
        }
        if (id == _selectedId)
            _productText.text = $"{product[id]} / {maxProduct[id]}";
        SaveData();
    }

    private IEnumerator DisplayTimeRemaining()
    {
        while (_nowTimeBonus > 0)
        {
            string timeString = FormatTime(_nowTimeBonus);
            _timeBonusText.text = timeString;
            yield return new WaitForSeconds(1f);
            _nowTimeBonus--;
            SaveData();
        }
        _timeBonusText.text = "";
        _plusSpeedButton.interactable = true;
        SaveData();
    }

    private string FormatTime(int seconds)
    {
        int hours = seconds / 3600;
        int minutes = seconds % 3600 / 60;
        int secs = seconds % 60;

        return $"{hours:D2}:{minutes:D2}:{secs:D2}";
    }
}
