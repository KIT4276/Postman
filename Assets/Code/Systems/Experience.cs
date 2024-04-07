using System;

public class Experience : ISavedProgress
{
    public event Action<float, float> ChangeExperienceValue;
    public event Action<float> ChangeExperienceLevel;

    public float ExperienceValue { get; private set; }
    public float ExperienceLevel { get; private set; }
    public float TargetXP { get; private set; }

    private readonly int _xpIncreaseStep;
    private readonly float _xpForDeliver;
    private readonly float _xpForKilling;

    public Experience(float targetXP, int xpIncreaseStep, int xpForDeliver, int xpForKilling, EnemyFactory enemyFactory, 
        DeliveredParcelsCounter parcelsCounter)
    {
        TargetXP = targetXP;
        _xpIncreaseStep = xpIncreaseStep;
        _xpForDeliver = xpForDeliver;
        _xpForKilling = xpForKilling;

        parcelsCounter.ChangeCount += CangeXPForDeliver;
        enemyFactory.DeadSumEnemyEvent += CangeXPForKilling;
    }

    public void UpdateProgress(PlayerProgress progress)
    {
        progress.ExperienceData.ExperienceValue = ExperienceValue;
        progress.ExperienceData.ExperienceLevel = ExperienceLevel;
    }

    public void LoadProgress(PlayerProgress progress)
    {
        ExperienceValue = progress.ExperienceData.ExperienceValue;
        ExperienceLevel = progress.ExperienceData.ExperienceLevel;

        ChangeExperienceLevel?.Invoke(ExperienceLevel);
        ChangeExperienceValue?.Invoke(ExperienceValue, TargetXP);
    }

    private void CangeXPForKilling() => 
        CangeXP(_xpForKilling);

    private void CangeXPForDeliver() => 
        CangeXP(_xpForDeliver);

    private void CangeXP(float value)
    {
        ExperienceValue += value;

        if (ExperienceValue >= TargetXP)
        {
            ExperienceLevel++;
            ExperienceValue = 0;
            TargetXP +=  TargetXP * _xpIncreaseStep /100;
            ChangeExperienceLevel?.Invoke(ExperienceLevel);
        }

        ChangeExperienceValue?.Invoke(ExperienceValue, TargetXP);
    }
}
