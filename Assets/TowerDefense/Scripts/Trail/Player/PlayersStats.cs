using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayersStats : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI MoneyDisplayText;
    [SerializeField] private int StartingMoney;
    private int currentMoney;

    void Start()
    {
        currentMoney = StartingMoney;
        MoneyDisplayText.SetText($"${StartingMoney}");
    }


    public bool HasEnoughMoney(float amount)
    {
        bool result = currentMoney >= amount;
        Debug.Log($"HasEnoughMoney: {currentMoney} >= {amount} ? {result}");
        return result;
    }

    // Add money
    public void AddMoney(int MoneyToAdd)
    {
        if (MoneyToAdd > 0)
        {
            currentMoney += MoneyToAdd;
            MoneyDisplayText.SetText($"${currentMoney}");
        }
    }

    // Subtract money (used when placing a tower)
    public bool SubtractMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            MoneyDisplayText.SetText($"${currentMoney}");
            return true;
        }
        return false;
    }

    public int GetMoney()
    {
        return currentMoney;
    }
}
