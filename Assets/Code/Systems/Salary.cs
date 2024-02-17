using System;

public class Salary : ISavedProgress
{
    private Post _post;
    private float _salaryAmount = 50; // move to stats

    public event Action ManyChangeE;

    public float Money { get; private set; }

    public Salary(Post post)
    {
        _post = post;

        _post.AddressEnterE += SalaryPayment;
    }

    public void ToSpand(float value)
    {
        Money -= value;
        ManyChangeE?.Invoke();
    }

    private void SalaryPayment()
    {
        Money += _salaryAmount;
        ManyChangeE?.Invoke();
    }


    public void UpdateProgress(PlayerProgress progress) => 
        progress.MoneyData.Money = Money;

    public void LoadProgress(PlayerProgress progress) => 
        Money = progress.MoneyData.Money;
}
