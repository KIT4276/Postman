using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] private Image ImageCurrent;

    private void Awake()
    {
        if (ImageCurrent == null)
            ImageCurrent = GetComponent<Image>();
    }

    public void SetValue(float current, float max)
    {
        if (ImageCurrent == null)
            return;

        if (max <= 0)
        {
            ImageCurrent.fillAmount = 0;
            return;
        }

        ImageCurrent.fillAmount = Mathf.Clamp01(current / max);
    }
}
