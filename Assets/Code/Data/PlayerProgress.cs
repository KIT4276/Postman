using System;

[Serializable]
public class PlayerProgress
{
    public State PlayerState;
    public WorldData WorldData;
    public Stats PlayerStats;
    public KillData KillData;
    public PostData PostData;
    public MoneyData MoneyData;
    public InfectedData InfectedData;
    public ExperienceData ExperienceData;

    public PlayerProgress(string initialLevel)
    {
        WorldData = new WorldData(initialLevel);
        PlayerState = new State();
        PlayerStats = new Stats();
        KillData = new KillData();
        PostData = new PostData();
        MoneyData = new MoneyData();
        InfectedData = new InfectedData();
        ExperienceData = new ExperienceData();
    }
}
