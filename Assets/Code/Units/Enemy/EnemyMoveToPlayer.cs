using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class EnemyMoveToPlayer : Follow
{
    [SerializeField]
    private float MinDistance = 1;
    [SerializeField]
    private NavMeshAgent Agent;

    [Inject]
    private GameFactory _gameFactory;

    private void Start()
    {
        if (_gameFactory.PlayerGameObject != null)
            InitializePlayerTransform();
        else
            _gameFactory.PlayerCreated += PlayerCreated;
    }

    private void Update()
    {
        if (Initialised() && HeroNotReached() && Agent.isActiveAndEnabled)
            Agent.destination = _playerTransform.position;
    }

    private bool Initialised() =>
        _playerTransform != null;

    private void PlayerCreated() =>
       InitializePlayerTransform();

    private void InitializePlayerTransform() =>
        _playerTransform = _gameFactory.PlayerGameObject.transform;

    private bool HeroNotReached() =>
        Vector3.Distance(Agent.transform.position, _playerTransform.position) >= MinDistance;
}
