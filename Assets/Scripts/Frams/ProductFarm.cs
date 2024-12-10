using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ProductFarm : MonoBehaviour
{
    public static event Action<int> UpdateFarm;
    public static event Action<int, int> CollectFarm;

    [SerializeField] private WaterFarm _waterFarm;

    [SerializeField] private TMP_Text _productText;
    [SerializeField] private int _selectedId;
    public float[] timeToPlusProduct;
    public int[] maxProduct;
    public int[] product;
    public float[] multipleTime;

    private bool _isUpdated = false;

    private Coroutine[] _plusProductCor;
    private bool _isFirst = true;

    private void Start(){
        if (_isFirst){
            timeToPlusProduct[0] = 36000 / 1000 / multipleTime[0];
            for (int i = 1; i < timeToPlusProduct.Length; i++)
                timeToPlusProduct[i] = timeToPlusProduct[0];
        }
        _plusProductCor = new Coroutine[timeToPlusProduct.Length];
        _productText.text = $"{product[_selectedId]} / {maxProduct[_selectedId]}";
    }

    private void OnEnable(){
        WaterFarm.UpdateFarm += PlusProductFarm;
        MethodsFarm.CollectProduct += CollectProduct;
        SelectFarm.SetSelectedId += UpdateSelectedId;
        UpgradesFarm.UpgradeMaxProductFarm += UpdateSelectedId;
        BuyFarm.BuyNewFarmEvent += UpdateNewBuy;
    }

    private void OnDisable(){
        WaterFarm.UpdateFarm -= PlusProductFarm;
        MethodsFarm.CollectProduct -= CollectProduct;
        SelectFarm.SetSelectedId -= UpdateSelectedId;
        UpgradesFarm.UpgradeMaxProductFarm -= UpdateSelectedId;
        BuyFarm.BuyNewFarmEvent -= UpdateNewBuy;
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
    }

    private void PlusProductFarm(int id){
        if (_waterFarm.percentWater[id] > 0 && product[id] < maxProduct[id]){
            if (_plusProductCor[id] != null)
                StopCoroutine(_plusProductCor[id]);
            _plusProductCor[id] = StartCoroutine(PlusProductFarmCor(id));
        }
        else
            StopCoroutine(_plusProductCor[id]);
    }

    public void ColculateTimeToPlus(int id){
        timeToPlusProduct[id] = 36000 / 1000 / multipleTime[id];
    }

    private void CollectProduct(int id){
        CollectFarm?.Invoke(id, product[id]);
        product[id] = 0;
        _productText.text = $"{product[id]} / {maxProduct[id]}";
        _isUpdated = false;
        UpdateFarm?.Invoke(_selectedId);
    }

    private IEnumerator PlusProductFarmCor(int id){
        yield return new WaitForSeconds(timeToPlusProduct[id]);
        
        product[id]++;
        if (id == _selectedId)
            _productText.text = $"{product[id]} / {maxProduct[id]}";
        if (product[id] < maxProduct[id]){
             _plusProductCor[id] = StartCoroutine(PlusProductFarmCor(id));
            if (product[id] >= maxProduct[id] / 10 && !_isUpdated){
                UpdateFarm?.Invoke(_selectedId);
                _isUpdated = true;
            }
        }
        else
            UpdateFarm?.Invoke(_selectedId);
    }
}
