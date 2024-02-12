using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyAnimator))]
public class AnimateAlongAgent : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent Agent;
    [SerializeField]
    private EnemyAnimator Animator;
    [SerializeField]
    private float MinSpeed = 0.1f;

    private void Update()
    {
        if (ShouldMove())
            Animator.Move(Agent.velocity.magnitude);
        else
            Animator.StopMoving();
    }

    private bool ShouldMove() =>
        Agent.velocity.magnitude >= MinSpeed && Agent.remainingDistance > Agent.radius;
}
