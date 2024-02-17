using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;

    private Salary _salary;

    public void SetSalary(Salary salary) =>
        _salary = salary;

    private void Start()
    {
        AddMoney();
        _salary.SalaryPaymentE += AddMoney;
    }

    private void AddMoney() => 
        _text.text = _salary.Money.ToString();

    private void OnDestroy()
    {
        _salary.SalaryPaymentE -= AddMoney;
    }
}
