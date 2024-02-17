using UnityEngine;
using UnityEngine.UI;

public class InfectionPanel : MonoBehaviour
{
    [SerializeField]
    public Image ImageCurrent;

    private PlayerInfection _infection;

    public void SetInfection(PlayerInfection infection) => 
        _infection = infection;

    private void Update()// move to public method
    {
        ImageCurrent.fillAmount = _infection.InfectedValue/100;
    }
}
