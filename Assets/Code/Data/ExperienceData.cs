using System;
using UnityEngine.Serialization;

[Serializable]
public class ExperienceData
{
    public float ExperienceValue;

    [FormerlySerializedAs("ExperienceLevel")]
    public float Milestone;
}
