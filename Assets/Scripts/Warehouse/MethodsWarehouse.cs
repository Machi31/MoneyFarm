using System;
using UnityEngine;

public class MethodsWarehouse : MonoBehaviour
{
    public static event Action SendProductMethod;

    public void SendProduct(){
        SendProductMethod?.Invoke();
    }
}
