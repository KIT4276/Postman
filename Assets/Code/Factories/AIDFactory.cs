using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AIDFactory : IGameplayFactory
{
    [Inject] private readonly AIDTrigger.Pool _aidPool;
    [Inject] private readonly AIDParent _parent;

    public List<AIDTrigger> Aids { get; private set; } = new List<AIDTrigger>();

    public event Action ChangeAIDCount;

    public AIDTrigger SpawnAID(Transform spawnTransform, float value)
    {
        AIDTrigger aid = _aidPool.Spawn();
        Aids.Add(aid);
        aid.transform.SetPositionAndRotation(spawnTransform.position, spawnTransform.rotation);
        aid.transform.parent = _parent.transform;
        aid.SetRecoveryHealth(value);

        ChangeAIDCount?.Invoke();

        return aid;
    }

    public void DespawnAID(AIDTrigger aid)
    {
        _aidPool.Despawn(aid);
        Aids.Remove(aid);
        ChangeAIDCount?.Invoke();
    }
}


