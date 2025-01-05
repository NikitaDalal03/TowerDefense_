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

    public void AddMoney(int MoneyToAdd)
    {
        currentMoney += MoneyToAdd;
        MoneyDisplayText.SetText($"${currentMoney}");
    }

    public int GetMoney()
    {
        return currentMoney;
    }
}
