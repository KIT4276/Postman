using TMPro;
using UnityEngine;

public class DeliveredParcelsPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;

    private DeliveredParcelsCounter _counter;

    public void SetCounter(DeliveredParcelsCounter counter) => 
        _counter = counter;

    private void Start()
    {
        UpdateCount();
        _counter.ChangeCount += UpdateCount;
    }

    private void UpdateCount() => 
        _text.text = _counter.DeliveredParcelsCount.ToString();

    private void OnDestroy() => 
        _counter.ChangeCount -= UpdateCount;
}
