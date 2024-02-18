using System;

public class DeliveredParcelsCounter : ISavedProgress
{

    private readonly ParcelGenerator _generator;
    public int DeliveredParcelsCount { get; private set; }

    public event Action ChangeCount;

    public DeliveredParcelsCounter(ParcelGenerator generator)
    {
        _generator = generator;

        _generator.GenerateAddressE += Counter;
    }

    private void Counter()
    {
        DeliveredParcelsCount++;
        ChangeCount?.Invoke();
    }

    public void UpdateProgress(PlayerProgress progress) => 
        progress.PostData.DeliveredParcelsCount = DeliveredParcelsCount;

    public void LoadProgress(PlayerProgress progress) => 
        DeliveredParcelsCount = progress.PostData.DeliveredParcelsCount;
}
