using System;
using UnityEngine.UI;

[Serializable]
public class PlayerProgress
{
    public State HeroState;
    public WorldData WorldData;
    public Stats HeroStats;
    public KillData KillData;
    public PostData PostData;

    public PlayerProgress(string initialLevel)
    {
        WorldData = new WorldData(initialLevel);
        HeroState = new State();
        HeroStats = new Stats();
        KillData = new KillData();
        PostData = new PostData();
    }
}
