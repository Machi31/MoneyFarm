using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GamePush;
using UnityEngine;

public class GemMarket : MonoBehaviour
{
    [SerializeField] private GameObject _gemMarketWindow;

    private float _fadeDuaration = 0.5f;

    private void OnEnable() {
        MethodsFarm.OpenGemMarket += OpenGemMarket;
        GP_Ads.OnRewardedReward += OnRewarded;
    }

    private void OnDisable() {
        MethodsFarm.OpenGemMarket -= OpenGemMarket;
        GP_Ads.OnRewardedReward -= OnRewarded;
    }

    public void BuyGem(string productId){
        GP_Payments.Purchase(productId, OnPurchaseSuccess, OnPurchaseError);
    }

    private void OnPurchaseSuccess(string productId){
        switch (productId){
            case "7337":
                MoneyAndGems.InstanceMG.PlusGem(100);
            break;
            case "7338":
                MoneyAndGems.InstanceMG.PlusGem(400);
            break;
            case "7339":
                MoneyAndGems.InstanceMG.PlusGem(700);
            break;
        }
    }

    private void OnPurchaseError() => Debug.Log("PURCHASE: ERROR");

    private void OpenGemMarket() => _gemMarketWindow.transform.DOMove(new Vector3(0, 0, 0), _fadeDuaration);

    public void GetGemForAd(){
        GP_Ads.ShowRewarded("GemReward");
    }

    private void OnRewarded(string arg0)
    {
        if (arg0 == "GemReward")
            MoneyAndGems.InstanceMG.PlusGem(10);
    }
}
