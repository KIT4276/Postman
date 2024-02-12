using UnityEngine;

public class Backpack : MonoBehaviour
{
    [SerializeField]
    private GameObject _backpack;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PostAddressTrigger>() != null)
            _backpack.SetActive(true);
        else if(other.GetComponent<AddressTrigger>() != null)
            _backpack.SetActive(false);
    }
}