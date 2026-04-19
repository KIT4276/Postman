using UnityEngine;
using UnityEngine.UI;

public class InfectionPanel : MonoBehaviour
{
    [SerializeField] private Image ImageCurrent;

    private PlayerInfection _infection;

    private void Awake()
    {
        if (ImageCurrent == null)
            ImageCurrent = FindChildComponent<Image>("Infection_bar");
    }

    public void SetInfection(PlayerInfection infection) =>
        _infection = infection;

    private void LateUpdate()
    {
        if (_infection == null || ImageCurrent == null)
            return;

        ImageCurrent.fillAmount = Mathf.Clamp01(_infection.InfectedValue / 100);
    }

    private T FindChildComponent<T>(string childName) where T : Component
    {
        foreach (Transform child in GetComponentsInChildren<Transform>(true))
            if (child.name == childName && child.TryGetComponent(out T component))
                return component;

        return null;
    }
}
