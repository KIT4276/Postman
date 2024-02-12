using UnityEngine;

[RequireComponent(typeof(Attack))]
public class CheckAttackRange : MonoBehaviour
{
    [SerializeField]
    private Attack Attack;
    [SerializeField]
    private TriggerObserver TriggerObserver;

    private void Start()
    {
        TriggerObserver.TriggerEnter += TriggerEnter;
        TriggerObserver.TriggerExit += TriggerExit;

        Attack.DisableAttack();
    }

    private void TriggerEnter(Collider obj) =>
        Attack.EnableAttack();

    private void TriggerExit(Collider obj) =>
        Attack.DisableAttack();
}
