using System;

public class Experience : ISavedProgress
{
    private const float MilestoneReward = 100;

    public event Action<float, float> ChangeExperienceValue;
    public event Action<float> ChangeMilestone;

    public float ExperienceValue { get; private set; }
    public float Milestone { get; private set; }
    public float TargetXP { get; private set; }

    private readonly int _xpIncreaseStep;
    private readonly float _xpForKilling;
    private readonly Salary _salary;

    public Experience(float targetXP, int xpIncreaseStep, int xpForKilling, EnemyFactory enemyFactory, Salary salary)
    {
        TargetXP = targetXP;
        _xpIncreaseStep = xpIncreaseStep;
        _xpForKilling = xpForKilling;
        _salary = salary;

        enemyFactory.DeadSumEnemyEvent += CangeXPForKilling;
    }

    public void UpdateProgress(PlayerProgress progress)
    {
        progress.ExperienceData.ExperienceValue = ExperienceValue;
        progress.ExperienceData.Milestone = Milestone;
    }

    public void LoadProgress(PlayerProgress progress)
    {
        ExperienceValue = progress.ExperienceData.ExperienceValue;
        Milestone = progress.ExperienceData.Milestone;

        ChangeMilestone?.Invoke(Milestone);
        ChangeExperienceValue?.Invoke(ExperienceValue, TargetXP);
    }

    private void CangeXPForKilling() => 
        CangeXP(_xpForKilling);

    private void CangeXP(float value)
    {
        ExperienceValue += value;

        if (ExperienceValue >= TargetXP)
        {
            Milestone++;
            ExperienceValue = 0;
            TargetXP +=  TargetXP * _xpIncreaseStep /100;
            _salary.AddMoney(MilestoneReward);
            ChangeMilestone?.Invoke(Milestone);
        }

        ChangeExperienceValue?.Invoke(ExperienceValue, TargetXP);
    }
}
