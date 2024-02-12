using UnityEngine;

public abstract class Follow : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 1;

    protected Transform _playerTransform;
    protected Vector3 _positionToLook;
    protected bool _isLookAtPlayer;

    public void RotateTowardsHero() => 
        RotateAroundY();

    protected void RotateAroundY() =>
       transform.rotation = Quaternion.Euler(transform.rotation.x, SmoothedRotation(transform.rotation, 
           UpdatePositionToLookAt()).eulerAngles.y, transform.rotation.z);

    protected Vector3 UpdatePositionToLookAt()
    {
        Vector3 positionDiff = _playerTransform.position - transform.position;
        _positionToLook = new Vector3(positionDiff.x, transform.position.y, positionDiff.z);
        return _positionToLook;
    }

    private Quaternion SmoothedRotation(Quaternion rotation, Vector3 positionToLook) =>
       Quaternion.Lerp(rotation, TargetRotation(positionToLook), SpeedFactor());

    private Quaternion TargetRotation(Vector3 position) =>
        Quaternion.LookRotation(position);

    private float SpeedFactor() =>
       _rotationSpeed * Time.deltaTime;
}

