using TMPro;
using UnityEngine;

public class ExperienceUI : MonoBehaviour
{
    [SerializeField] private Bar _xpBar;
    [SerializeField] private TextMeshProUGUI _text;

    private Experience _experience;

    private void Awake()
    {
        if (_xpBar == null)
            _xpBar = FindChildComponent<Bar>("kill_bar");
    }

    public void Init(Experience experience)
    {
        if (_experience != null)
            Unsubscribe();

        _experience = experience;

        if (_experience == null)
            return;

        ChangeValue(_experience.ExperienceValue, _experience.TargetXP);
        ChangeMilestone(_experience.Milestone);

        _experience.ChangeExperienceValue += ChangeValue;
        _experience.ChangeMilestone += ChangeMilestone;
    }

    private void ChangeMilestone(float milestone) =>
        _text?.SetText(milestone.ToString());

    private void ChangeValue(float current, float target) =>
        _xpBar?.SetValue(current, target);

    private void OnDestroy() =>
        Unsubscribe();

    private void Unsubscribe()
    {
        if (_experience == null)
            return;

        _experience.ChangeExperienceValue -= ChangeValue;
        _experience.ChangeMilestone -= ChangeMilestone;
        _experience = null;
    }

    private T FindChildComponent<T>(string childName) where T : Component
    {
        foreach (Transform child in GetComponentsInChildren<Transform>(true))
            if (child.name == childName && child.TryGetComponent(out T component))
                return component;

        return null;
    }
}
