using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyAndGems : MonoBehaviour
{
    public static MoneyAndGems InstanceMG { get; private set; }

    [SerializeField] private TMP_Text[] _moneyTxt;
    [SerializeField] private TMP_Text _totalMoneyTxt;
    [SerializeField] private TMP_Text[] _gemsTxt;

    public int totalMoney;
    public int money;
    public int gems;

    private void Start() {
        if (!GameManager.Instance._isFirst){
            totalMoney = PlayerPrefs.GetInt("TotalMoney");
            money = PlayerPrefs.GetInt("Money");
            gems = PlayerPrefs.GetInt("Gems");
        }
        UpdateText();
    }

    private void Awake()
    {
        if (InstanceMG != null && InstanceMG != this)
        {
            Destroy(gameObject);
            return;
        }

        InstanceMG = this;
    }

    private void UpdateText(){
        _totalMoneyTxt.text = totalMoney.ToString();
        for (int i = 0; i < _moneyTxt.Length; i++){
            _moneyTxt[i].text = money.ToString();
            _gemsTxt[i].text = gems.ToString();
        }
    }

    public void MinusMoney(int count){
        money -= count;
        UpdateText();
        SaveData();
    }
    public void MinusGem(int count){
        gems -= count;
        UpdateText();
        SaveData();
    }

    public void PlusMoney(int count){
        totalMoney += count;
        money += count;
        UpdateText();
        SaveData();
    }
    public void PlusGem(int count){
        gems += count;
        UpdateText();
        SaveData();
    }

    private void SaveData() {
        PlayerPrefs.SetInt("TotalMoney", totalMoney);
        PlayerPrefs.SetInt("Money", money);
        PlayerPrefs.SetInt("Gems", gems);
    }
}
