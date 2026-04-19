using UnityEngine;

public class Backpack : MonoBehaviour
{
    [SerializeField]
    private GameObject _backpack;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PostAddressTrigger>(out _))
            _backpack.SetActive(true);
        else if (other.TryGetComponent<AddressTrigger>(out _))
            _backpack.SetActive(false);
    }
}
