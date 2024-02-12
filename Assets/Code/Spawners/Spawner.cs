using UnityEngine;

public abstract class Spawner : MonoBehaviour, ISavedProgress
{
    public abstract void LoadProgress(PlayerProgress progress);

    public abstract void UpdateProgress(PlayerProgress progress);
}
