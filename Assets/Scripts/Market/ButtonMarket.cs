using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMarket : MonoBehaviour
{
    public static event Action<int, int> GiveFromMarket;

    [SerializeField] private GameObject _coin;
    [SerializeField] private GameObject _gem;

    [SerializeField] private GameObject _sellProductWindow;

    [SerializeField] private Image _sellableProduct;
    [SerializeField] private Sprite[] _sellablesProducts;

    [SerializeField] private TMP_Text _costText;
    [SerializeField] private TMP_Text _countText;

    [SerializeField] private int _idBox; 
    [SerializeField] private int _idResource; 
    [SerializeField] private int _countResource;

    private float _fadeDuaration = 0.5f;

    private void OnEnable(){
        Market.SendProductToBox += GetButtonStats;
    }
    private void OnDisable(){
        Market.SendProductToBox -= GetButtonStats;
    }

    private void GetButtonStats(int id, int idResourse, int countResourse){
        if(id == _idBox){
            _idResource = idResourse;
            _countResource = countResourse;
        }
    }

    public void StartSell(){
        _sellProductWindow.transform.DOMove(new Vector3(0, 0, 0), _fadeDuaration);
        if (_idResource < 6){
            _coin.SetActive(true);
            _gem.SetActive(false);
        }
        else{
            _coin.SetActive(false);
            _gem.SetActive(true);
        }
        _sellableProduct.sprite = _sellablesProducts[_idResource];
        _countText.text = $"{_countResource}";
        _costText.text = $"{_countResource * Market.InstanceMarket._costProduct[_idResource]}";
        GiveFromMarket?.Invoke(_idResource, _countResource);
    }
}
