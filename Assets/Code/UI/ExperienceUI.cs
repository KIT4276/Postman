using TMPro;
using UnityEngine;

public class ExperienceUI : MonoBehaviour
{
    [SerializeField] private Bar _xpBar;
    [SerializeField] private TextMeshProUGUI _text;

    public void Init(Experience experience)
    {
        ChangeValue(experience.ExperienceValue, experience.TargetXP);
        CangeLvl(experience.ExperienceLevel);

        experience.ChangeExperienceValue += ChangeValue;
        experience.ChangeExperienceLevel += CangeLvl;
    }

    private void CangeLvl(float lvl) => 
        _text.text = lvl.ToString();

    private void ChangeValue(float current, float target) => 
        _xpBar.SetValue(current, target);
}
