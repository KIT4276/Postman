using UnityEngine;
using UnityEngine.UI;

public class InfectionPanel : MonoBehaviour
{
    [SerializeField]
    public Image ImageCurrent;

    private Infection _infection;

    public void SetInfection(Infection infection) => 
        _infection = infection;

    private void Update()// move to public method
    {
        ImageCurrent.fillAmount = _infection.InfectedValue;
    }
}
