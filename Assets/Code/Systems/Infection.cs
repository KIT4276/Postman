public class Infection : ISavedProgress
{
    public float InfectedValue { get; private set; }


    public Infection()
    {
        //todo
    }

    public void LoadProgress(PlayerProgress progress) => 
        progress.InfectedData.InfectedValue = InfectedValue;

    public void UpdateProgress(PlayerProgress progress) => 
        InfectedValue = progress.InfectedData.InfectedValue;
}
