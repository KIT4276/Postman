using TMPro;
using UnityEngine;

public class DeliveredParcelsPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private DeliveredParcelsCounter _counter;

    private void Awake()
    {
        if (_text == null)
            _text = FindChildText("DeliveredText (TMP)");
    }

    public void SetCounter(DeliveredParcelsCounter counter) =>
        _counter = counter;

    private void Start()
    {
        if (_counter == null || _text == null)
            return;

        UpdateCount();
        _counter.ChangeCount += UpdateCount;
    }

    private void UpdateCount() =>
        _text.text = _counter.DeliveredParcelsCount.ToString();

    private void OnDestroy()
    {
        if (_counter != null)
            _counter.ChangeCount -= UpdateCount;
    }

    private TextMeshProUGUI FindChildText(string childName)
    {
        foreach (Transform child in GetComponentsInChildren<Transform>(true))
            if (child.name == childName && child.TryGetComponent(out TextMeshProUGUI text))
                return text;

        return null;
    }
}
