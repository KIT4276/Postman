using UnityEngine;
using Zenject;

public class RotateToPlayer : Follow
{
    [SerializeField]
    private float Speed;

    [Inject]
    private GameFactory _gameFactory;

    private void Start()
    {
        if (HeroExists())
            InitialiseHeroTransform();
        else
            _gameFactory.PlayerCreated += InitialiseHeroTransform;
    }

    private void Update()
    {
        if (Initialized())
            RotateTowardsHero();
    }

    private bool HeroExists() =>
        _gameFactory.PlayerGameObject != null;

    private void InitialiseHeroTransform() =>
        _playerTransform = _gameFactory.PlayerGameObject.transform;

    private bool Initialized() =>
        _playerTransform != null;
}
