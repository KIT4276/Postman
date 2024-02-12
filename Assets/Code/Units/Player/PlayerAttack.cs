using UnityEngine;

//[RequireComponent(typeof(PlayerAnimator))]
//[RequireComponent(typeof(CharacterController))]
public class PlayerAttack : MonoBehaviour, ISavedProgressReader
{
    private const string Hittable = "Hittable";
    public PlayerAnimator HeroAnimator;
    public CharacterController CharacterController;
    public GameObject Bat;

    private IInputService _input;

    private static int _layerMask;
    private Collider[] _hits = new Collider[3];
    private Stats _stats;

    public void Init(IInputService input) =>
        _input = input;

    private void Awake() =>
        _layerMask = 1 << LayerMask.NameToLayer(Hittable);

    private void Update()
    {
        if (_input.IsAttackButtonUp())// && HeroAnimator.IsAttacking)  пока не разберёмся с изменением State у HeroAnimator
            HeroAnimator.PlayAttack();
    }

    private void OnAttack()
    {
        for (int i = 0; i < Hit(); i++)
            _hits[i].transform.parent.GetComponent<IHealth>().ChangeHealth( -_stats.Damage);
    }

    private void OnAttackEnded()
    {
        //todo
    }

    public void LoadProgress(PlayerProgress progress) =>
        _stats = progress.HeroStats;

    private int Hit()
    {

        return Physics.OverlapSphereNonAlloc(StartPoint() + transform.forward, _stats.DamageRadius, _hits, _layerMask);
    }

    //private Vector3 StartPoint() =>
    //    new Vector3(transform.position.x, CharacterController.center.y, transform.position.z);

    private Vector3 StartPoint() =>
        Bat.transform.position;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(StartPoint(), 0.2f);
    }
}
