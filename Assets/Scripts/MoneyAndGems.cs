using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyAndGems : MonoBehaviour
{
    public static MoneyAndGems InstanceMG { get; private set; }

    [SerializeField] private int _money;
    [SerializeField] private int _gems;

    private void Awake()
    {
        if (InstanceMG != null && InstanceMG != this)
        {
            Destroy(gameObject);
            return;
        }

        InstanceMG = this;
    }

    public void MinusMoney(int count){
        _money -= count;
    }
    public void MinusGem(int count){
        _gems -= count;
    }

    public void PlusMoney(int count){
        _money += count;
    }
    public void PlusGem(int count){
        _gems += count;
    }
}
