using System;

public interface IRewardedAdService
{
    event Action RewardGranted;
    event Action<bool> RewardFlowCompleted;

    bool IsBusy { get; }

    bool TryShowHealReward();
}
