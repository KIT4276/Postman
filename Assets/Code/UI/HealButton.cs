using UnityEngine;
using UnityEngine.UI;

public class HealButton : MonoBehaviour
{
    [SerializeField] private float _healingPrice;
    [SerializeField] private Button _healForMoneyButton;
    [SerializeField] private Button _healForAdButton;

    private Salary _salary;
    private Healing _healing;
    private PersistantStaticData _staticData;
    private IRewardedAdService _rewardedAdService;

    private void Awake()
    {
        if (_healForMoneyButton == null)
            _healForMoneyButton = FindChildComponent<Button>("HealForMoneyB");

        if (_healForAdButton == null)
            _healForAdButton = FindChildComponent<Button>("HealForAdB");

        _healForMoneyButton?.onClick.AddListener(ToHeal);
        _healForAdButton?.onClick.AddListener(ToHealForAd);
    }

    public void Init(Salary salary, Healing healing, PersistantStaticData staticData,
        IRewardedAdService rewardedAdService, ILocalizationService localizationService)
    {
        _salary = salary;
        _healing = healing;
        _staticData = staticData;
        _rewardedAdService = rewardedAdService;

        _healingPrice = _staticData.HealingPrice;
    }

    public void ToHeal()
    {
        if (_salary == null || _healing == null)
            return;

        if (_salary.Money < _healingPrice)
            return;

        _salary.ToSpand(_healingPrice);
        _healing.ToHeal();
    }

    public void ToHealForAd()
    {
        if (_rewardedAdService == null)
            return;

        _rewardedAdService.TryShowHealReward();
    }

    private void OnDestroy()
    {
        _healForMoneyButton?.onClick.RemoveListener(ToHeal);
        _healForAdButton?.onClick.RemoveListener(ToHealForAd);
    }

    private T FindChildComponent<T>(string childName) where T : Component
    {
        foreach (Transform child in GetComponentsInChildren<Transform>(true))
            if (child.name == childName && child.TryGetComponent(out T component))
                return component;

        return null;
    }
}
