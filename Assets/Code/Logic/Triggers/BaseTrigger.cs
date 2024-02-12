using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BaseTrigger : MonoBehaviour
{
    public void Awake() => 
        GetComponent<Collider>().isTrigger = true;
}