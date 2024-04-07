using System;

public class Salary : ISavedProgress
{
    private readonly Post _post;
    private readonly PersistantStaticData _staticData;

    private readonly float _salaryAmount;

    public event Action ManyChangeE;

    public float Money { get; private set; }

    public Salary(Post post, PersistantStaticData staticData)
    {
        _post = post;
        _staticData = staticData;

        _salaryAmount = _staticData.SalaryAmount;
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
