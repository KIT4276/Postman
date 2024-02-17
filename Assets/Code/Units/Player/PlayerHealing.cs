using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealing : MonoBehaviour
{
    [SerializeField]
    private PlayerInfection _infection;

    private Healing _healing;

    public void SetHealing(Healing healing) => 
        _healing = healing;

    private void Start()
    {
        _healing.ToHealE += Hial;
    }

    public void Hial() => 
        _infection.StopInfection();


}
