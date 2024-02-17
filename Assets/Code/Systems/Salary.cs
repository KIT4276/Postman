using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salary : ISavedProgress
{
    private Post _post;
    private float _salaryAmount = 50; // move to stats

    public event Action SalaryPaymentE;

    public float Money { get; private set; }

    public Salary(Post post)
    {
        _post = post;

        _post.AddressEnterE += SalaryPayment;
    }

    private void SalaryPayment()
    {
        Money += _salaryAmount;
        SalaryPaymentE?.Invoke();
    }

    public void UpdateProgress(PlayerProgress progress)
    {
       //todo
    }

    public void LoadProgress(PlayerProgress progress)
    {
        //todo
    }
}
