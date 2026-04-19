using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BaseTrigger : MonoBehaviour
{
    [SerializeField] private Collider _collider;

    public void Awake()
    {
        if (_collider == null)
            _collider = GetComponent<Collider>();

        _collider.isTrigger = true;
    }
}
