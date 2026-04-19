using TMPro;
using UnityEngine;

public class MoneyPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private Salary _salary;

    private void Awake()
    {
        if (_text == null)
            _text = FindChildText("Coins_back");
    }

    public void SetSalary(Salary salary) =>
        _salary = salary;

    private void Start()
    {
        if (_salary == null || _text == null)
            return;

        AddMoney();
        _salary.ManyChangeE += AddMoney;
    }

    private void AddMoney() =>
        _text.text = _salary.Money.ToString();

    private void OnDestroy()
    {
        if (_salary != null)
            _salary.ManyChangeE -= AddMoney;
    }

    private TextMeshProUGUI FindChildText(string parentName)
    {
        foreach (Transform child in GetComponentsInChildren<Transform>(true))
            if (child.name == parentName)
                return child.GetComponentInChildren<TextMeshProUGUI>(true);

        return null;
    }
}
