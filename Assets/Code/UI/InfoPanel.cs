using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject _panel;

    public void ButtoneClick() => 
        _panel.SetActive(false);

}
