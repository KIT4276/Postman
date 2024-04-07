using System.Collections.Generic;
using UnityEngine;

public class SpawnersRotate : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawners = new();

    private void Start()
    {
        foreach (Transform spawner in _spawners)
            spawner.rotation = Quaternion.Euler(transform.rotation.x, Random.Range(0, 360), transform.rotation.z);
    }
}
