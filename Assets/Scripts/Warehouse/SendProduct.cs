using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SendProduct : MonoBehaviour
{
    [SerializeField] private GameObject _coin;
    [SerializeField] private GameObject _gem;
    [SerializeField] private Image _productImage;
    [SerializeField] private Sprite[] _productsImages;

    [SerializeField] private TMP_Text _costText;
    [SerializeField] private TMP_Text _countText;

    private void OnEnable(){
        Warehouse.SendId += NeedBox;
    }

    private void OnDisable(){
        Warehouse.SendId -= NeedBox;
    }

    private void NeedBox(int id, int count){
        if (id < 6){
            _coin.SetActive(true);
            _gem.SetActive(false);
        }
        else{
            _coin.SetActive(false);
            _gem.SetActive(true);
        }
        _productImage.sprite = _productsImages[id];
        _costText.text = $"{count * Market.InstanceMarket._costProduct[id]}";
        _countText.text = $"{count}";
    }
}
