using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] private Image ImageCurrent;

    public void SetValue(float current, float max) =>
        ImageCurrent.fillAmount = current / max;
}
