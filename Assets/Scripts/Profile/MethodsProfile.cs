using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MethodsProfile : MonoBehaviour
{
    public static event Action<int> BuyFarm;

    public void BuyFarmMethod(int id){
        BuyFarm?.Invoke(id);
    }
}
