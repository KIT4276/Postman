using System;

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

    public void UpdateProgress(PlayerProgress progress) => 
        progress.MoneyData.Money = Money;

    public void LoadProgress(PlayerProgress progress) => 
        Money = progress.MoneyData.Money;
}
