using System.Linq;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField]
    private EnemyAnimator _animator;
    [SerializeField]
    private float _attackCoolDown = 3f;
    [SerializeField]
    private float _clevage = 0.5f;
    [SerializeField]
    private float _effectiveDistance = 0.5f;
    [SerializeField]
    private float _indentationY = 0.5f;
    [SerializeField]
    private float _damage = 10;
    [SerializeField]
    private Follow _follow;
    
    [SerializeField]
    private GameObject _bat;

    private const string Player = "Player";

    private float _currentAttackCoolDown;
    private bool _isAttacking;
    private int _layerMask;

    private Collider[] _hits = new Collider[1];
    private bool _attackIsActive;


    private void Awake()
    {
        _currentAttackCoolDown = _attackCoolDown;
        _layerMask = 1 << LayerMask.NameToLayer(Player);
    }

    public void Restart()
    {
        _currentAttackCoolDown = _attackCoolDown;
        _isAttacking = false;
    }

    private void Update()
    {
        UpdatwCoolDown();

        if (CanAttack())
            StartAttack();
    }

    private void OnAttack()
    {
        if (Hit(out Collider hit))
        {
            PhysicsDebug.DrawDebug(_bat.transform.position, _clevage, 0.3f);
            if (hit.transform.TryGetComponent<IHealth>(out IHealth playerHealth))
                playerHealth.ChangeHealth( - _damage);
        }
    }

    private void OnAttackEnded()
    {
        Debug.Log("OnAttackEnded");
        _currentAttackCoolDown = _attackCoolDown;
        _isAttacking = false;
    }

    public void EnableAttack() =>
        _attackIsActive = true;

    public void DisableAttack() =>
        _attackIsActive = false;

    private bool Hit(out Collider hit)
    {
        int hitCount = Physics.OverlapSphereNonAlloc(_bat.transform.position, _clevage, _hits, _layerMask);

        hit = _hits.FirstOrDefault();

        return hitCount > 0;
    }

    private void UpdatwCoolDown()
    {
        if (!CoolDownIsUp())
            _currentAttackCoolDown -= Time.deltaTime;
    }

    private void StartAttack()
    {
        _follow.RotateTowardsHero();

        _animator.PlayAttack();
        _isAttacking = true;
    }

    private bool CanAttack()
    {
        return _attackIsActive && !_isAttacking && CoolDownIsUp();
    }

    private bool CoolDownIsUp() =>
        _currentAttackCoolDown <= 0f;
}
