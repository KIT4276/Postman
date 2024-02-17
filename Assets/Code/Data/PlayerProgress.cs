using System;

[Serializable]
public class PlayerProgress
{
    public State HeroState;
    public WorldData WorldData;
    public Stats PlayerStats;
    public KillData KillData;
    public PostData PostData;
    public MoneyData MoneyData;

    public PlayerProgress(string initialLevel)
    {
        WorldData = new WorldData(initialLevel);
        HeroState = new State();
        PlayerStats = new Stats();
        KillData = new KillData();
        PostData = new PostData();
        MoneyData = new MoneyData();
    }
}
